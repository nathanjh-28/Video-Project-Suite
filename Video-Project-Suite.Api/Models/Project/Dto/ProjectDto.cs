using System;

namespace Video_Project_Suite.Api.Models;

public class ProjectDto
{
    // this is supposed to be a shorted version of Project model but for now it is identical

    // id
    public int Id { get; set; }

    // short_name
    public string ShortName { get; set; } = string.Empty;

    // Title, the default value is the ShortName
    public string Title { get; set; } = string.Empty;

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

    public int MilestoneId { get; set; }

    // Project Type to be implemented later on
    // for now we are use a string for type
    public string Type { get; set; } = string.Empty;

    // Users associated with project
    // this will be a join table in the database

}
