using System;
using Microsoft.EntityFrameworkCore;

namespace Video_Project_Suite.Api.Data;

// Extensions.cs
//
// This file contains extension methods for the IHost interface to create and initialize the database.
// It provides a method to create the database if it does not exist and seed it with initial data.
//

public static class Extensions
{
    public static void CreateDbIfNotExists(this IHost host)
    {
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();
                DbInitializer.Initialize(context);
            }
        }
    }

    public static void ResetDatabase(this IHost host)
    {
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();

                // Check if using in-memory database
                bool isInMemory = context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory";

                if (!isInMemory)
                {
                    // Drop all tables by dropping and recreating the schema (PostgreSQL only)
                    Console.WriteLine("Dropping all tables...");
                    context.Database.ExecuteSqlRaw("DROP SCHEMA public CASCADE;");
                    context.Database.ExecuteSqlRaw("CREATE SCHEMA public;");

                    // Grant permissions (PostgreSQL requires this after recreating schema)
                    context.Database.ExecuteSqlRaw("GRANT ALL ON SCHEMA public TO nathanjh;");
                    context.Database.ExecuteSqlRaw("GRANT ALL ON SCHEMA public TO public;");
                }
                else
                {
                    // For in-memory database, just delete it
                    Console.WriteLine("Clearing in-memory database...");
                    context.Database.EnsureDeleted();
                }

                // Recreate the database schema
                Console.WriteLine("Creating database schema...");
                context.Database.EnsureCreated();

                // Seed with initial data
                Console.WriteLine("Seeding database...");
                DbInitializer.Initialize(context);

                Console.WriteLine("Database reset complete!");
            }
        }
    }
}
