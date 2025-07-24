using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Video_Project_Suite.Api.Data;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Models.Dto;

namespace Video_Project_Suite.Api.Services;

// AuthService.cs
//
// This file implements the IAuthService interface, providing methods for user authentication operations.
// It contains the AuthService class which implements methods for user registration, login, token refresh, and user role alteration.
// The AuthService class uses the IAuthService interface to define the methods for user authentication.

public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
{
    ///////////////////////////////////////////////////////////////////////////
    //
    // Interface Methods
    //
    ///////////////////////////////////////////////////////////////////////////

    // Login

    // This method is used to register a new user in the system.
    public async Task<TokenResponseDto?> LoginAsync(UserDto request)
    {
        var user = await context.User.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null)
        {
            return null; // User not found
        }

        if (string.IsNullOrEmpty(request.Password))
        {
            return null; // Invalid password
        }

        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
            == PasswordVerificationResult.Failed)
        {
            return null; // Invalid password
        }
        var response = new TokenResponseDto
        {
            AccessToken = CreateToken(user),
            RefreshToken = await GenerateandSaveRefreshToken(user)
        };
        return response;
    }

    // Register

    // This method is used to register a new user in the system.
    public async Task<User?> RegisterAsync(RegisterUserDto request)
    {
        if (await context.User.AnyAsync(u => u.Username == request.Username))
        {
            return null; // User already exists
        }

        var user = new User();
        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(user, request.Password);

        user.Username = request.Username;
        user.PasswordHash = hashedPassword;
        user.Email = request.Email;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;


        context.User.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

    // Refresh Tokens

    // This method is used to refresh the access token using the refresh token.
    public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
    {
        var user = await ValidateRefreshToken(request.UserId, request.RefreshToken);
        if (user == null)
        {
            return null; // Invalid refresh token
        }

        return await CreateTokenResponse(user);
    }

    // Alter User Role

    // This method is used to alter the role of a user, only accessible by Admins.
    public async Task<User?> AlterUserRoleAsync(AlterUserRoleDto request)
    {
        var user = await context.User.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null)
        {
            return null; // User not found
        }
        user.Role = request.NewRole;
        context.User.Update(user);
        await context.SaveChangesAsync();
        return user; // User role altered successfully
    }

    // Change Password
    public async Task<User?> ChangePasswordAsync(ChangePasswordDto request)
    {
        var user = await context.User.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null)
        {
            return null; // User not found
        }

        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.OldPassword)
            == PasswordVerificationResult.Failed)
        {
            return null; // Invalid password
        }
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.NewPassword);
        context.User.Update(user);
        await context.SaveChangesAsync();
        return user;
    }


    // delete account
    public async Task<User?> DeleteAccountAsync(int userId)
    {
        var user = await context.User.FindAsync(userId);
        if (user == null)
        {
            return null; // User not found
        }

        context.User.Remove(user);
        await context.SaveChangesAsync();
        return user; // User deleted successfully
    }

    // Get all Users
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await context.User.ToListAsync();
    }

    // Get a user by ID
    public async Task<User?> GetUserByIdAsync(int userId)
    {
        var user = await context.User.FindAsync(userId);
        if (user == null)
        {
            return null; // User not found
        }
        return user; // Return the found user
    }

    // Update User Account
    public async Task<User?> UpdateUserAsync(int userId, RegisterUserDto user)
    {
        var existingUser = await context.User.FindAsync(userId);
        if (existingUser == null)
        {
            return null; // User not found
        }

        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;

        context.User.Update(existingUser);
        await context.SaveChangesAsync();
        return existingUser; // Return the updated user
    }

    // Get Users with a specific role
    public async Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName)
    {
        return await context.User.Where(u => u.Role == roleName).ToListAsync();
    }

    ///////////////////////////////////////////////////////////////////////////
    //
    // Helper methods
    //
    ///////////////////////////////////////////////////////////////////////////

    // Create a JWT token

    // This method creates a JWT token for the user.
    private string CreateToken(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration["AppSettings:Issuer"],
            audience: configuration["AppSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    // Generate Refresh Token

    // This method generates a new refresh token.
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }

    // Generate and Save Refresh Token

    private async Task<string> GenerateandSaveRefreshToken(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await context.SaveChangesAsync();
        return refreshToken;

    }

    // Validate Refresh Token
    private async Task<User?> ValidateRefreshToken(int userId, string refreshToken)
    {
        var user = await context.User.FindAsync(userId);
        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime < DateTime.Now)
        {
            return null; // Invalid refresh token
        }
        return user;
    }

    // Create Token Response

    // This method creates a token response containing the access token and refresh token.
    private async Task<TokenResponseDto> CreateTokenResponse(User? user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null");
        }
        var response = new TokenResponseDto
        {
            AccessToken = CreateToken(user),
            RefreshToken = await GenerateandSaveRefreshToken(user)
        };
        return response;
    }

}
