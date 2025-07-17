using System;

namespace Video_Project_Suite.Api.Models.Dto;

public class ChangePasswordDto
{
    public string Username { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
