using System;
using System.Collections.Generic;
using ProjectModel = Video_Project_Suite.Api.Models.Project;

namespace Video_Project_Suite.Api.Models.Dto;

public class UserDetailDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Role { get; set; } = string.Empty;

    // list of potential roles on projects
    public List<string> ProjectRoles { get; set; } = new List<string>();



    // // Skip Navigation for convenience
    // public List<ProjectModel.Project> Projects { get; set; } = new List<ProjectModel.Project>();

    // // Direct Access to assignments
    // public List<UserProject.UserProject> Assignments { get; set; } = new List<UserProject.UserProject>();
}
