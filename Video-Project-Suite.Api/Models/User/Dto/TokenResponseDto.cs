using System;

namespace Video_Project_Suite.Api.Models.Dto;

// TokenResponseDto.cs
//
// This class represents the response containing tokens after user authentication.
// It includes properties for the access token and refresh token.
//

public class TokenResponseDto
{

    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
