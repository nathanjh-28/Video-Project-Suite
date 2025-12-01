using System;

namespace Video_Project_Suite.Api.Models.Milestone;

public class Milestone
{
    // id
    public int Id { get; set; }

    // name
    public string Name { get; set; } = string.Empty;

    // position among milestones (changeable)
    public int Position { get; set; }

    // Projects associated with this milestone

    public ICollection<Video_Project_Suite.Api.Models.Project.Project> Projects { get; set; } = null!;


}
