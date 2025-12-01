using System;
using System.Collections.Generic;
using UserModel = Video_Project_Suite.Api.Models.User;
using MilestoneModel = Video_Project_Suite.Api.Models.Milestone;


// add restrictions to properties
using System.ComponentModel.DataAnnotations;

namespace Video_Project_Suite.Api.Models.Project;

public class Project
{
    // id
    public int Id { get; set; }

    // short_name used as a handle for the project
    [Required]
    [StringLength(100)]
    public string ShortName { get; set; } = string.Empty;

    // Formal Title, the default value is the ShortName
    public string Title { get; set; } = string.Empty;

    // Focus - What is the primary focus of this project?
    public string Focus { get; set; } = string.Empty;

    // Scope - What are the boundaries of this project?
    public string Scope { get; set; } = string.Empty;

    // price_per_unit

    public decimal PricePerUnit { get; set; }

    // qty_of_units
    public int QtyOfUnits { get; set; }

    // expense budget
    public decimal ExpenseBudget { get; set; }

    // expense summary
    public string ExpenseSummary { get; set; } = string.Empty;

    // comments

    public string Comments { get; set; } = string.Empty;

    // start_date
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    // end_date
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    // Milestone - For tracking projects on Kanban board
    public int MilestoneId { get; set; } // Foreign Key
    public MilestoneModel.Milestone Milestone { get; set; } = null!;

    // Project Type to be implemented later on
    // for now we are use a string for type
    public string Type { get; set; } = string.Empty;

    // Users assigned to the project
    // this will be a join table in the database

    // Skip Navigation for convenience
    public List<UserModel.User> Users { get; set; } = new List<UserModel.User>();

    // Direct Access to assignments
    public List<UserProject.UserProject> Assignments { get; set; } = new List<UserProject.UserProject>();
}
