using System;

namespace Video_Project_Suite.Api.Models.UserProject;

using Video_Project_Suite.Api.Models.Project;
using Video_Project_Suite.Api.Models.User;

public class UserProjectDto
{
    public int ProjectId { get; set; }
    public int UserId { get; set; }

    // role

    public string Role { get; set; } = string.Empty;

}
