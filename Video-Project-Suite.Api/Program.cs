
using Scalar.AspNetCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Video_Project_Suite.Api.Services;
using Video_Project_Suite.Api.Data;

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

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // SQLite database for storing user data
        builder.Services.AddSqlite<AppDbContext>("Data Source=app.db");

        // Add authentication Services

        // Add Auth Service
        builder.Services.AddScoped<IAuthService, AuthService>();

        // Configure the HTTP request pipeline.
        var app = builder.Build();

        // Configure the HTTP request pipeline for development.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(); // Map the Scalar API reference

        }

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
