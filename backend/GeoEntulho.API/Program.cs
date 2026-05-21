using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GeoEntulho.API.Services;
using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using Firebase.Auth;

var builder = WebApplication.CreateBuilder(args);

// Configurar logging para diagnóstico
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Inicializar Firebase usando environment variables
var projectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID");
var apiKey = Environment.GetEnvironmentVariable("FIREBASE_API_KEY");

if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine("[GeoEntulho] ⚠️  Firebase not configured. Development mode expected.");
    Console.WriteLine($"  FIREBASE_PROJECT_ID: {(!string.IsNullOrEmpty(projectId) ? "✓" : "✗")}");
    Console.WriteLine($"  FIREBASE_API_KEY: {(!string.IsNullOrEmpty(apiKey) ? "✓" : "✗")}");
}
else
{
    Console.WriteLine($"[GeoEntulho] ✓ Firebase configured. Project: {projectId}");
}

// Registrar FirebaseService como singleton
builder.Services.AddSingleton<IFirebaseService>(provider =>
{
    return new FirebaseService(projectId, apiKey, provider.GetRequiredService<ILogger<FirebaseService>>());
});

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"] ?? Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("JWT Key not configured");
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

app.Run();
