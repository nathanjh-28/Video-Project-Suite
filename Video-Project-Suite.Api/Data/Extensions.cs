using System;

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
}
