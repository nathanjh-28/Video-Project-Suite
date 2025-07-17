using System;

namespace Video_Project_Suite.Api.Data;

// DbInitializer.cs
//
// This class is responsible for initializing the database with default data.
// It checks if the database already contains any users and seeds it with initial data if not.
//


public class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        if (context.User.Any())
        {
            return;
        }

        // Seed the database with initial data here if needed




        context.SaveChanges();
    }
}
