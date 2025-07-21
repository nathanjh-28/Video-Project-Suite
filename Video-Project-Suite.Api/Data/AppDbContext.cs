using System;
using Microsoft.EntityFrameworkCore;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Models.Project;

namespace Video_Project_Suite.Api.Data;

// AppDbContext.cs
//
// This class represents the application's database context.
// It inherits from DbContext and provides access to the User entity.
//

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> User => Set<User>();
    public DbSet<Project> Project => Set<Project>();
}
