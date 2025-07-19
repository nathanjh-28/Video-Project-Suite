// AuthController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Models.Dto;
using Video_Project_Suite.Api.Services;

namespace Video_Project_Suite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        // Register a new user
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterUserDto request)
        {
            var user = await authService.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest("Registration failed.");
            }

            // we should return a dto instead of user but for now we return the user
            return Ok(user);
        }

        // Login an existing user
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var result = await authService.LoginAsync(request);
            if (result == null)
            {
                return BadRequest("Invalid username or password");
            }

            return Ok(result);
        }

        // Logout an existing user
        // could implement a blackout list of tokens

        // Change Password Route
        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto request)
        {
            var result = await authService.ChangePasswordAsync(request);
            if (result == null)
            {
                return BadRequest("Failed to change password.");
            }
            return Ok();
        }

        // Forgot Password Route

        // alter user role
        [Authorize(Roles = "Admin")]
        [HttpPost("alter-user-role")]
        public async Task<ActionResult> AlterUserRole(AlterUserRoleDto request)
        {
            // Implement alter user role logic here
            var result = await authService.AlterUserRoleAsync(request);
            if (result == null)
            {
                return BadRequest("Failed to alter user role.");
            }
            return Ok();
        }

        // Refresh Tokens Route
        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokensAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized("Invalid refresh token.");

            return Ok(result);
        }
    }
}
