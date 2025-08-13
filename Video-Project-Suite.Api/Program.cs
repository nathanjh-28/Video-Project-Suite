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
                    .WithOrigins("http://localhost:5173") // React app URL
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

        // Convert Render's DATABASE_URL format to Npgsql format
        if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgresql://"))
        {
            var uri = new Uri(connectionString.Replace("postgresql://", "postgres://"));
            var userInfo = uri.UserInfo.Split(':');
            connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
        }
        else
        {
            connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        }


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
                ValidateIssuerSigningKey = true
            };
        });

        // Add Auth Service
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();

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
        // Use Cors
        app.UseCors("AllowReactApp");

        // Enable HTTPS redirection
        app.UseHttpsRedirection();

        // Enable authorization middleware
        app.UseAuthorization();

        // create and seed the database if it does not exist
        app.CreateDbIfNotExists();

        // Map the controllers to the request pipeline
        app.MapControllers();

        // Run the application
        app.Run();
    }
}

// public partial class Program { }
