using System;

namespace Video_Project_Suite.Api.Models.Dto;



public class AlterUserRoleDto
{
    public string Username { get; set; } = string.Empty;
    public string NewRole { get; set; } = string.Empty;
}
