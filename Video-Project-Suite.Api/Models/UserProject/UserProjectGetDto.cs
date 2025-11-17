using System;

namespace Video_Project_Suite.Api.Models.UserProject;

using Video_Project_Suite.Api.Models.Project;
using Video_Project_Suite.Api.Models.User;

public class UserProjectGetDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int UserId { get; set; }

    public string Role { get; set; } = string.Empty;

    public DateOnly AssignedAt { get; set; }
    public DateOnly? RemovedAt { get; set; }

    public string ProjectShortName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

}
