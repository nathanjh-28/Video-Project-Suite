using System;
using Microsoft.AspNetCore.Identity;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Models.Dto;
using Video_Project_Suite.Api.Models.Milestone;
using Video_Project_Suite.Api.Models.Project;
using Video_Project_Suite.Api.Models.UserProject;
using Video_Project_Suite.Api.Models.User;


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

        #region UserSeedList

        // Seed the database with default user data
        var UserSeedList = new[]
        {
            // UserSeedList is an array of hash maps containing default user data.
            // Each user has a username and a role.
            new {
                Username = "admin",
                Role="admin",
                Email="nathan@horizonstudio.com",
                FirstName="Nathan",
                LastName="Harris",
                ProjectRoles = new List<string> { "admin", "producer", "editor", "client" }
                },
            // Producer
            new {
                Username = "oliv",
                Role="User",
                Email="olivia.martinez@horizonstudio.com",
                FirstName="Olivia",
                LastName="Martinez",
                ProjectRoles = new List<string> { "producer"}
                },
            new {
                Username = "liam",
                Role="User",
                Email="liam.thompson@horizonstudio.com",
                FirstName="Liam",
                LastName="Thompson",
                ProjectRoles = new List<string> { "producer"}
                },
            // Editor
            new {
                Username = "soph",
                Role="User",
                Email="sophia.johnson@horizonstudio.com",
                FirstName="Sophia",
                LastName="Johnson",
                ProjectRoles = new List<string> {"editor"}
                },
            new {
                Username = "jim",
                Role="User",
                Email="james.anderson@horizonstudio.com",
                FirstName="James",
                LastName="Anderson",
                ProjectRoles = new List<string> {"editor"}
                },

            // Clients
            new {
                Username = "emily",
                Role="User",
                Email="emily.roberts@clientemail.com",
                FirstName="Emily",
                LastName="Roberts",
                ProjectRoles = new List<string> {"client"}
                },
            new {
                Username = "michael",
                Role="User",
                Email="michael.brown@clientemail.com",
                FirstName="Michael",
                LastName="Brown",
                ProjectRoles = new List<string> {"client"}
                },
            new {
                Username = "chloe",
                Role="User",
                Email="chloe.wilson@clientemail.com",
                FirstName="Chloe",
                LastName="Wilson",
                ProjectRoles = new List<string> {"client"}
                },
            new {
                Username = "noah",
                Role="User",
                Email="noah.taylor@clientemail.com",
                FirstName="Noah",
                LastName="Taylor",
                ProjectRoles = new List<string> {"client"}
                },
            new {
                Username = "sarah",
                Role="User",
                Email="sarah.carter@clientemail.com",
                FirstName="Sarah",
                LastName="Carter",
                ProjectRoles = new List<string> {"client"}
                },
            new {
                Username = "elijah",
                Role="User",
                Email="elijah.adams@clientemail.com",
                FirstName="Elijah",
                LastName="Adams",
                ProjectRoles = new List<string> {"client"}
                },
            new {
                Username = "ava",
                Role="User",
                Email="ava.mitchell@clientemail.com",
                FirstName="Ava",
                LastName="Mitchell",
                ProjectRoles = new List<string> {"client"}
                },
            new {
                Username = "jackson",
                Role="User",
                Email="jackson.perez@clientemail.com",
                FirstName="Jackson",
                LastName="Perez",
                ProjectRoles = new List<string> {"client"}
                },
            new {
                Username = "isabella",
                Role="User",
                Email="isabella.lee@clientemail.com",
                FirstName="Isabella",
                LastName="Lee",
                ProjectRoles = new List<string> {"client"}
                },
            new {
                Username = "oliver",
                Role="User",
                Email="oliver.young@clientemail.com",
                FirstName="Oliver",
                LastName="Young",
                ProjectRoles = new List<string> {"client"}
                }
        };

        #endregion

        foreach (var u in UserSeedList)
        {
            var user = new User();
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, "1234");
            user.Username = u.Username;
            user.PasswordHash = hashedPassword;
            user.Role = u.Role;
            user.Email = u.Email;
            user.FirstName = u.FirstName;
            user.LastName = u.LastName;
            user.ProjectRoles = u.ProjectRoles;

            context.User.Add(user);

        }


        #region MilestoneSeedList

        var milestoneSeedList = new[]
        {
            new Milestone
            {
                Name = "Prospect",
                Position = 1
            },
            new Milestone
            {
                Name = "Bidding",
                Position = 2
            },
            new Milestone
            {
                Name = "Development",
                Position = 3
            },
            new Milestone
            {
                Name = "Pre-Production",
                Position = 4
            },
            new Milestone
            {
                Name = "Production",
                Position = 5
            },
            new Milestone
            {
                Name = "Post-Production",
                Position = 6
            },
            new Milestone
            {
                Name = "Invoiced",
                Position = 7
            },
            new Milestone
            {
                Name = "Completed",
                Position = 8
            }
        };

        #endregion

        // Save milestones FIRST before adding anything else (required for foreign key constraint)
        foreach (var m in milestoneSeedList)
        {
            context.Milestones.Add(m);
        }
        context.SaveChanges();

        #region ProjectSeedList

        // Seed the Database with default project data
        var ProjectSeedList = new[]
        {
            new Project
            {
                Title = "Sunrise Coffee Launch Campaign",
                ShortName = "sunrise coffee",
                Focus = "Promotional campaign for the launch of a new coffee brand.",
                Scope = "Three 30-second social media videos and one 60-second TV spot.",
                PricePerUnit = 2500,
                QtyOfUnits = 4,
                ExpenseBudget = 7500,
                ExpenseSummary = "Studio rental, talent fees, camera crew, and editing",
                Comments = "Focus on lifestyle and morning routines.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(1),
                MilestoneId = 1, // Prospect
                Type = "Promotional Videos"
            },
            new Project
            {
                Title = "GreenTech Documentary Series – Episode 1",
                ShortName = "greentech_doc1",
                Focus = "Documentary on sustainable technology startups.",
                Scope = "15-minute documentary featuring three startups.",
                PricePerUnit = 6000,
                QtyOfUnits = 1,
                ExpenseBudget = 4500,
                ExpenseSummary = "Travel, interviews, equipment rental, editing",
                Comments = "Use natural light where possible.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-30),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(45),
                MilestoneId = 1, // Prospect
                Type = "Short Documentary"
            },

            new Project
            {
                Title = "Atlas Apparel 2025 Summer Lookbook",
                ShortName = "atlas_summer",
                Focus = "Fashion promotional video for online store.",
                Scope = "5 short videos showcasing different outfits.",
                PricePerUnit = 1800,
                QtyOfUnits = 5,
                ExpenseBudget = 3500,
                ExpenseSummary = "Model fees, wardrobe, lighting setup, studio time",
                Comments = "Keep videos under 45 seconds each.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(4),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(24),
                MilestoneId = 1,
                Type = "Short Promo Videos"
            },

            new Project
            {
                Title = "City Museum Promo Spot",
                ShortName = "museum_promo",
                Focus = "60-second promotional video for a local museum.",
                Scope = "One social media and one TV-ready version.",
                PricePerUnit = 3200,
                QtyOfUnits = 2,
                ExpenseBudget = 2500,
                ExpenseSummary = "Location fees, actor/host, camera crew",
                Comments = "Highlight new exhibition.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(9),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(29),
                MilestoneId = 1, // Preproduction
                Type = "Promotional Videos"
            },

            new Project
            {
                Title = "Apex Industrial Safety Training Video",
                ShortName = "apex_safety",
                Focus = "Instructional video for employee training.",
                Scope = "20-minute video, animated and live-action segments.",
                PricePerUnit = 5000,
                QtyOfUnits = 1,
                ExpenseBudget = 4000,
                ExpenseSummary = "Animation team, live shoot, editing, voiceover",
                Comments = "Must comply with OSHA standards.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-20),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(25),
                MilestoneId = 6, // Post-Production
                Type = "Training Video"
            },

            new Project
            {
                Title = "WildRiver Adventure Travel Promo",
                ShortName = "wildriver_travel",
                Focus = "90-second promo for a travel company specializing in river expeditions.",
                Scope = "Outdoor shoot over two days, multiple river locations.",
                PricePerUnit = 3500,
                QtyOfUnits = 1,
                ExpenseBudget = 6000,
                ExpenseSummary = "Travel, drone footage, permits, guide fees",
                Comments = "Emphasize action and adventure.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(7),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(19),
                MilestoneId = 1, // Preproduction
                Type = "Promo Video"
            },

            new Project
            {
                Title = "SilverLine Automotive Commercial",
                ShortName = "silverline_auto",
                Focus = "30-second TV commercial for a new car model.",
                Scope = "Commercial with one outdoor and one studio scene.",
                PricePerUnit = 8000,
                QtyOfUnits = 1,
                ExpenseBudget = 10000,
                ExpenseSummary = "Car rental, location, talent, cinematography, editing",
                Comments = "Include slow-motion driving shots.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(11),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(27),
                MilestoneId = 1, // Preproduction
                Type = "Commercial"
            },

            new Project
            {
                Title = "HarvestFest Event Highlights",
                ShortName = "harvestfest_highlights",
                Focus = "3-minute recap video of an annual festival.",
                Scope = "Coverage of live performances, audience, and interviews.",
                PricePerUnit = 2000,
                QtyOfUnits = 1,
                ExpenseBudget = 1500,
                ExpenseSummary = "Crew, camera rental, travel, editing",
                Comments = "Highlight festival atmosphere.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-25),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-15),
                MilestoneId = 6, // Post-Production
                Type = "Event Highlight Video"
            },

            new Project
            {
                Title = "EcoHome Product Demo Videos",
                ShortName = "ecohome_demo",
                Focus = "Demonstration videos for three eco-friendly home products.",
                Scope = "Three 2-minute videos, each focusing on a product.",
                PricePerUnit = 1500,
                QtyOfUnits = 3,
                ExpenseBudget = 3000,
                ExpenseSummary = "Product setup, lighting, editing, voiceover",
                Comments = "Emphasize environmental benefits.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(17),
                MilestoneId = 1, // Preproduction
                Type = "Product Demo Videos"
            },

            new Project
            {
                Title = "Horizon Health Documentary",
                ShortName = "horizon_health_doc",
                Focus = "Documentary on innovative health clinics in urban areas.",
                Scope = "20-minute documentary with interviews and B-roll footage.",
                PricePerUnit = 7000,
                QtyOfUnits = 1,
                ExpenseBudget = 6500,
                ExpenseSummary = "Travel, interviews, camera crew, editing, music licensing",
                Comments = "Emphasize patient stories and community impact.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-40),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(40),
                MilestoneId = 6, // Post-Production
                Type = "Short Documentary"
            },

            new Project
            {
                Title = "PeakTech Startup Pitch Videos",
                ShortName = "peaktech_pitch",
                Focus = "Series of 3 pitch videos for investors.",
                Scope = "3 x 90-second videos, one per product line.",
                PricePerUnit = 2200,
                QtyOfUnits = 3,
                ExpenseBudget = 5500,
                ExpenseSummary = "Studio rental, lighting, editing, motion graphics",
                Comments = "Keep concise and persuasive.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(2),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(15),
                MilestoneId = 1, // Preproduction
                Type = "Investor Pitch Videos"
            },

            new Project
            {
                Title = "CityLights Branded Content Series",
                ShortName = "citylights_series",
                Focus = "Ongoing branded content for an urban lifestyle brand.",
                Scope = "Five 1–2 minute videos, episodic format.",
                PricePerUnit = 1800,
                QtyOfUnits = 5,
                ExpenseBudget = 8000,
                ExpenseSummary = "Talent, location, crew, editing, music",
                Comments = "Emphasize city lifestyle aesthetics.",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(45),
                MilestoneId = 1, // Preproduction
                Type = "Branded Content Series"
            }
        };

        #endregion

        foreach (var p in ProjectSeedList)
        {
            context.Project.Add(p);
        }

        // Save projects separately
        context.SaveChanges();

        var userProjectSeedList = new[]
        {
            // Assign admin to all projects as admin
            new UserProject
            {
                UserId = 1, // admin
                ProjectId = 1, // first project
                Role = "producer"
            },
            new UserProject
            {
                UserId = 8, // client
                ProjectId = 2, // second project
                Role = "client"
            }
            // Add more assignments as needed
        };

        foreach (var up in userProjectSeedList)
        {
            context.UserProject.Add(up);
        }


        context.SaveChanges();
    }
}
