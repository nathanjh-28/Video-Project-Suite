using System;

namespace Video_Project_Suite.Api.Models.Dto;

// UserDto.cs
//
// This file defines the UserDto class, which is a Data Transfer Object (DTO) used for user-related operations.
// The UserDto class is needed to encapsulate user data that will be sent over the network
// during user registration, login, and other user-related operations.
//

public class UserDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
