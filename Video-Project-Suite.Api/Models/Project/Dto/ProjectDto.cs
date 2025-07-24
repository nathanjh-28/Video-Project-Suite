using System;

namespace Video_Project_Suite.Api.Models;

public class ProjectDto
{
    // this is a shorted version of Project model for API GET responses

    // need list of users that can be associated with project

    // short_name
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

    // start date only
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    // end date day only
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    // TBD:

    // Milestone

    // Project Type

    // Users associated with project

}
