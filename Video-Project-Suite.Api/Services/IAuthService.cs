using System;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Models.Dto;

namespace Video_Project_Suite.Api.Services;

// IAuthService.cs
//
// This interface defines the contract for authentication services in the Video Project Suite API.
// It specifies methods for user registration, login, and token refresh operations.
//

public interface IAuthService
{
    // This method is used to register a new user in the system.
    Task<User?> RegisterAsync(RegisterUserDto request);
    Task<TokenResponseDto?> LoginAsync(UserDto request);

    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);

    // change password

    // forgot password

    // logout

    // delete account


}
