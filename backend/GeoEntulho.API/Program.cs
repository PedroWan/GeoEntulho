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

// Configurar logging para diagnóstico
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configure EF Core with connection string (MySQL)
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
builder.Services.AddScoped<IFirebaseService, SqlDataService>();

// JWT Configuration
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
        db.Database.Migrate();
        Console.WriteLine("[GeoEntulho] Database migrations applied.");
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
