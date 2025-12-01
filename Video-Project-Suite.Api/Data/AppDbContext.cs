using System;
using Microsoft.EntityFrameworkCore;
using Video_Project_Suite.Api.Models.User;
using Video_Project_Suite.Api.Models.Project;
using Video_Project_Suite.Api.Models.UserProject;
using Video_Project_Suite.Api.Models.Milestone;

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

    // Add DbSet for UserProject join table
    public DbSet<UserProject> UserProject => Set<UserProject>();

    public DbSet<Video_Project_Suite.Api.Models.Milestone.Milestone> Milestones { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Projects)
            .WithMany(p => p.Users)
            .UsingEntity<UserProject>(
                j => j.HasOne(pa => pa.Project)
                      .WithMany(p => p.Assignments)
                      .HasForeignKey(pa => pa.ProjectId),
                j => j.HasOne(pa => pa.User)
                      .WithMany(u => u.Assignments)
                      .HasForeignKey(pa => pa.UserId),
                j =>
                {
                    j.HasKey(pa => new { pa.Id }); // Primary key

                    // Only apply PostgreSQL-specific configuration if not using in-memory database
                    if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                    {
                        j.Property(pa => pa.AssignedAt).HasDefaultValueSql("now()"); // PostgreSQL
                    }

                    j.HasIndex(pa => new
                    {
                        pa.UserId,
                        pa.ProjectId,
                        pa.Role
                    }).IsUnique();
                });

        modelBuilder.Entity<Video_Project_Suite.Api.Models.Milestone.Milestone>()
            .HasMany(m => m.Projects)
            .WithOne(p => p.Milestone)
            .HasForeignKey(p => p.MilestoneId)
            .IsRequired();


    }


}
