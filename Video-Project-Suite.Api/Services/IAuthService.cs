using System;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Models.Dto;
using Video_Project_Suite.Api.Models.User;

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
    Task<User?> AlterUserRoleAsync(AlterUserRoleDto request);


    // change password
    Task<User?> ChangePasswordAsync(ChangePasswordDto request);

    // forgot password

    // logout

    // delete account
    Task<User?> DeleteAccountAsync(int userId);

    // Get all Users
    Task<IEnumerable<User>> GetAllUsersAsync();

    // Get a user by ID
    Task<User?> GetUserByIdAsync(int userId);

    // Update User Account
    Task<User?> UpdateUserAsync(int userId, RegisterUserDto user);

    // Get Users with a specific role
    Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName);


}
