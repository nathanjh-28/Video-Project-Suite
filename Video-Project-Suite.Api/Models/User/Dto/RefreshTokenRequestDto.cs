using System;

namespace Video_Project_Suite.Api.Models.Dto;

public class RefreshTokenRequestDto
{
    public int UserId { get; set; }
    public required string RefreshToken { get; set; }
}
