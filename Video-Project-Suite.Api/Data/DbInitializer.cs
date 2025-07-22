using System;
using Microsoft.AspNetCore.Identity;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Models.Dto;
using Video_Project_Suite.Api.Models.Project;

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

        // Seed the database with default user data
        var UserSeedList = new[]
        {
            // UserSeedList is an array of hash maps containing default user data.
            // Each user has a username and a role.
            new { Username = "admin", Role="admin"},
            new { Username = "producer1", Role="producer"},
            new { Username = "producer2", Role="producer" },
            new { Username = "client1", Role="client"},
            new { Username = "client2", Role="client"},
            new { Username = "client3", Role="client"},
            new { Username = "editor1", Role="editor"},
            new { Username = "editor2", Role="editor"}
        };


        foreach (var u in UserSeedList)
        {
            var user = new User();
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, "1234");

            user.Username = u.Username;
            user.PasswordHash = hashedPassword;
            user.Role = u.Role;

            context.User.Add(user);

        }

        #region ProjectSeedList

        // Seed the Database with default project data
        var ProjectSeedList = new[]
        {
            new Project
            {
                ShortName = "Project A",
                Focus = "Focus A",
                Scope = "Scope A",
                PricePerUnit = 100.0m,
                QtyOfUnits = 10,
                ExpenseBudget = 1000.0m,
                ExpenseSummary = "Expense Summary A",
                Comments = "Comments A",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1)
            },
            new Project
            {
                ShortName = "Project B",
                Focus = "Focus B",
                Scope = "Scope B",
                PricePerUnit = 200.0m,
                QtyOfUnits = 20,
                ExpenseBudget = 2000.0m,
                ExpenseSummary = "Expense Summary B",
                Comments = "Comments B",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(2)
            },
            new Project
            {
                ShortName = "Project C",
                Focus = "Focus C",
                Scope = "Scope C",
                PricePerUnit = 300.0m,
                QtyOfUnits = 30,
                ExpenseBudget = 3000.0m,
                ExpenseSummary = "Expense Summary C",
                Comments = "Comments C",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(3)
            }
        };

        #endregion

        foreach (var p in ProjectSeedList)
        {
            context.Project.Add(p);
        }


        context.SaveChanges();
    }
}
