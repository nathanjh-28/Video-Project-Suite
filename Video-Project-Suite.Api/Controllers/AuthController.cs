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

        // Change Password Route

        // Forgot Password Route

        // alter user role

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
