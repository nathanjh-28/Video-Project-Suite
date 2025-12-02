using Scalar.AspNetCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Video_Project_Suite.Api.Services;
using Video_Project_Suite.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Video_Project_Suite.Api;

// Program.cs
//
// This file is the entry point for the Video Project Suite API application.
// It sets up the web application, configures services, and defines the HTTP request pipeline.
// It also initializes the database and maps controllers to handle incoming requests.
//
// The Program.cs file is essential for bootstrapping the application, configuring services like authentication,
// and setting up the middleware pipeline.

public class Program
{
    public static void Main(string[] args)
    {
        // Create a web application builder
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();


        // Add Cors
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp",
                    builder => builder
                    .WithOrigins("http://localhost:5173", "https://video-project-suite.vercel.app")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
        });

        // For .NET 8, use AddEndpointsApiExplorer instead of AddOpenApi
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        // builder.Services.AddOpenApi();

        // SQLite database for storing user data
        // builder.Services.AddSqlite<AppDbContext>("Data Source=app.db");

        // PostgreSQL database for storing user data
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
        Console.WriteLine($"DATABASE_URL found: {!string.IsNullOrEmpty(connectionString)}");

        // Also check for Render's specific env var name
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable("DATABASE_PRIVATE_URL");
        }

        Console.WriteLine($"DATABASE_URL found: {!string.IsNullOrEmpty(connectionString)}");
        Console.WriteLine($"Raw DATABASE_URL length: {connectionString?.Length ?? 0}");

        // Add this to see ALL environment variables (temporarily for debugging)
        Console.WriteLine("Available environment variables:");
        foreach (var env in Environment.GetEnvironmentVariables().Keys)
        {
            Console.WriteLine($"  - {env}");
        }

        // Convert Render's DATABASE_URL format to Npgsql format
        if (!string.IsNullOrEmpty(connectionString))
        {
            if (connectionString.StartsWith("postgresql://") || connectionString.StartsWith("postgres://"))
            {
                try
                {
                    // Parse the URL
                    var uri = new Uri(connectionString.Replace("postgresql://", "postgres://"));
                    var userInfo = uri.UserInfo.Split(':');
                    var database = uri.AbsolutePath.TrimStart('/');

                    // Get port (default to 5432 if not specified)
                    var port = uri.Port > 0 ? uri.Port : 5432;

                    // Build Npgsql connection string
                    connectionString = $"Host={uri.Host};Port={port};Database={database};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
                    Console.WriteLine("Converted DATABASE_URL to Npgsql format");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to parse DATABASE_URL: {ex.Message}");
                    throw;
                }
            }
        }
        else
        {
            // Fallback to config file
            connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine("Using connection string from configuration");
        }

        Console.WriteLine($"Final connection string format: {connectionString?.Substring(0, Math.Min(30, connectionString?.Length ?? 0))}...");


        builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(connectionString));

        // Add authentication Services

        var jwtToken = builder.Configuration["AppSettings:Token"];
        if (string.IsNullOrEmpty(jwtToken))
        {
            // Use a dummy token for testing environments
            jwtToken = "DummyTokenForTestingPurposesOnly-MustBeLongEnough-1234567890ABCDEFGHIJKLMNOP";
            Console.WriteLine("WARNING: Using dummy JWT token - this should only happen in tests");
        }

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(Options =>
        {
            Options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["AppSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["AppSettings:Audience"],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtToken)),
                ValidateIssuerSigningKey = true,
                // ClockSkew = TimeSpan.Zero  // Remove the default 5-minute clock skew for testing purposes
            };

            Options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });

        // Add Services
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IUserProjectService, UserProjectService>();
        builder.Services.AddScoped<IMilestoneService, MilestoneService>();

        // Configure the HTTP request pipeline.
        var app = builder.Build();

        // Configure the HTTP request pipeline for development.
        if (app.Environment.IsDevelopment())
        {
            // https://guides.scalar.com/scalar/scalar-api-references/integrations/net-aspnet-core

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "/openapi/{documentName}.json";
            });
            app.MapScalarApiReference();

        }

        // Use Cors - MUST come before UseHttpsRedirection
        app.UseCors("AllowReactApp");

        // Enable HTTPS redirection
        app.UseHttpsRedirection();

        // Enable authentication middleware
        app.UseAuthentication();

        // Enable authorization middleware
        app.UseAuthorization();

        // create and seed the database if it does not exist
        app.CreateDbIfNotExists();

        // TEMPORARY: Uncomment the line below to reset the database with seed data
        // app.ResetDatabase();

        // Map the controllers to the request pipeline
        app.MapControllers();

        // Run the application
        app.Run();
    }
}

// public partial class Program { }
