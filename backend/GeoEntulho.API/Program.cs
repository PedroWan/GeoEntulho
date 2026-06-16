using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GeoEntulho.API.Services;
using GeoEntulho.API.Data;
using Microsoft.EntityFrameworkCore;

// Safe exception printer (placed early so global handlers can use it)
static void PrintExceptionSafe(Exception? ex)
{
    if (ex == null) return;
    try
    {
        Console.WriteLine($"[Unhandled] Exception Type: {ex.GetType().FullName}");
        Console.WriteLine($"[Unhandled] Message: {ex.Message}");
        if (!string.IsNullOrEmpty(ex.StackTrace)) Console.WriteLine("[Unhandled] StackTrace:\n" + ex.StackTrace);
        if (ex.InnerException != null)
        {
            Console.WriteLine("[Unhandled] InnerException:");
            PrintExceptionSafe(ex.InnerException);
        }
    }
    catch
    {
        try { Console.WriteLine("[Unhandled] Failed to print exception details."); } catch { }
    }
}

// Global handlers to capture exceptions that happen very early
AppDomain.CurrentDomain.UnhandledException += (s, e) =>
{
    try { PrintExceptionSafe(e.ExceptionObject as Exception ?? new Exception("Unhandled exception object")); } catch { }
};
TaskScheduler.UnobservedTaskException += (s, e) =>
{
    try { PrintExceptionSafe(e.Exception); } catch { }
    e.SetObserved();
};

var builder = WebApplication.CreateBuilder(args);

// Diagnostic: list loaded assemblies and runtime info (avoid touching types that trigger static initializers)
try
{
    Console.WriteLine($"[Diag] Runtime: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");
    var loaded = AppDomain.CurrentDomain.GetAssemblies()
        .Select(a => new { a.GetName().Name, a.GetName().Version })
        .OrderBy(a => a.Name);
    Console.WriteLine("[Diag] Loaded assemblies:");
    foreach (var a in loaded)
    {
        Console.WriteLine($"  - {a.Name} {a.Version}");
    }
}
catch (Exception ex)
{
    try { Console.WriteLine($"[Diag] Failed to list assemblies: {ex.Message}"); } catch { }
}

// Safely build the app and capture any exceptions during build
IHost? builtHost = null;
try
{
    // builder.Build() can trigger DI type initializers; wrap to capture errors
    builtHost = builder.Build();
}
catch (Exception ex)
{
    PrintExceptionSafe(ex);
    try
    {
        Console.WriteLine($"[Diag] Environment JWT_SECRET length: {(Environment.GetEnvironmentVariable("JWT_SECRET")?.Length ?? 0)}");
    }
    catch { }
    Environment.Exit(1);
}

// Configurar logging para diagnóstico
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configure EF Core with connection string (MySQL)
Console.WriteLine("[Startup] STEP: configuring DB");
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("[GeoEntulho] ⚠️  No DB connection string configured. Using in-memory for development.");
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("GeoEntulhoDev"));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}

// Register SQL-backed data service
Console.WriteLine("[Startup] STEP: registering SqlDataService");
builder.Services.AddScoped<IFirebaseService, SqlDataService>();

// JWT Configuration
Console.WriteLine("[Startup] STEP: configuring JWT");
var jwtSettings = builder.Configuration.GetSection("Jwt");
var envJwt = Environment.GetEnvironmentVariable("JWT_SECRET");
var configJwt = jwtSettings["Key"];
var jwtKey = !string.IsNullOrWhiteSpace(envJwt)
    ? envJwt
    : (!string.IsNullOrWhiteSpace(configJwt) ? configJwt : throw new InvalidOperationException("JWT Key not configured"));
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// CORS Configuration
Console.WriteLine("[Startup] STEP: configuring CORS");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        var environment = builder.Environment.EnvironmentName;
        var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:5173";
        
        if (environment == "Development")
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:3000", "http://localhost:3001")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
        else
        {
            policy.WithOrigins(frontendUrl)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
    });
});

Console.WriteLine("[Startup] STEP: adding controllers (Swagger temporarily disabled)");
builder.Services.AddControllers();
// Temporarily disabled for diagnostics: avoid initializing Swagger which may load additional assemblies
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builtHost!;

// Configure the HTTP request pipeline
// Swagger and SwaggerUI temporarily disabled for diagnostics
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
// else
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Apply EF Core migrations at startup (creates tables on Railway MySQL)
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        try
        {
            var providerName = db.Database.ProviderName ?? "(unknown)";
            Console.WriteLine($"[GeoEntulho] DB Provider: {providerName}");
            // Only attempt relational migrations when provider supports it
            if (db.Database.IsRelational())
            {
                db.Database.Migrate();
                Console.WriteLine("[GeoEntulho] Database migrations applied.");
            }
            else
            {
                Console.WriteLine("[GeoEntulho] Skipping migrations: non-relational provider.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GeoEntulho] Failed to apply migrations: {ex.Message}");
        }
    }

    app.Run();
}
catch (Exception ex)
{
    // Print useful details without relying on Exception.ToString()
    PrintExceptionSafe(ex);
    // Also print some environment diagnostics (mask secrets)
    try
    {
        var dbConn = Environment.GetEnvironmentVariable("DB_CONNECTION");
        Console.WriteLine($"[GeoEntulho] DB_CONNECTION present: {(!string.IsNullOrWhiteSpace(dbConn)).ToString()}");
        var jwt = Environment.GetEnvironmentVariable("JWT_SECRET");
        Console.WriteLine($"[GeoEntulho] JWT_SECRET length: {(jwt is null ? 0 : jwt.Length)}");
    }
    catch { }

    // Ensure non-zero exit so container stops
    Environment.Exit(1);
}
