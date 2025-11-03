using System;
using Video_Project_Suite.Api.Models.User;

namespace Video_Project_Suite.Api.Models.UserProject;

using Video_Project_Suite.Api.Models.Project;

public class UserProject
{

    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int UserId { get; set; }

    public Video_Project_Suite.Api.Models.User.User User { get; set; } = null!;

    public Project Project { get; set; } = null!;

    // datetime
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RemovedAt { get; set; } = null;

    // role

    public string Role { get; set; } = string.Empty;




}
