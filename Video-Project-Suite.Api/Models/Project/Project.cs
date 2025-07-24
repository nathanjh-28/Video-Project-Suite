using System;

// add restrictions to properties
using System.ComponentModel.DataAnnotations;

namespace Video_Project_Suite.Api.Models.Project;

public class Project
{
    // id
    public int Id { get; set; }

    // short_name
    [Required]
    [StringLength(100)]
    public string ShortName { get; set; } = string.Empty;

    // focus
    public string Focus { get; set; } = string.Empty;

    // scope

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
    // TBD:

    // Milestone

    // Project Type

    // Users associated with project
}
