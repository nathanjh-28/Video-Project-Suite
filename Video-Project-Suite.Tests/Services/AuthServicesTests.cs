using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Video_Project_Suite.Api.Data;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Models.Dto;
using Video_Project_Suite.Api.Services;
using Xunit;


namespace Video_Project_Suite.Api.Tests.Services
{
    public class AuthServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            // Setup in-memory database for testing
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Setup configuration mock
            _configuration = CreateConfiguration();


            _authService = new AuthService(_context, _configuration);
        }

        private IConfiguration CreateConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "AppSettings:Token", "this-is-a-test-secret-key-that-is-long-enough-for-hmac-sha512l;kjasd;lkjasdlkjfasldkjf;ldjsaf" },
                { "AppSettings:Issuer", "TestIssuer" },
                { "AppSettings:Audience", "TestAudience" }
            });

            return configurationBuilder.Build();
        }

        private User CreateTestUser(string username = "testuser", string email = "test@example.com")
        {
            var user = new User
            {
                Id = 1,
                Username = username,
                Email = email,
                FirstName = "Test",
                LastName = "User",
                Role = "User"
            };

            // Hash a test password
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, "TestPassword123!");

            return user;
        }


        public void Dispose()
        {
            _context.Dispose();
        }

        #region LoginAsync Tests

        // TRUE POSITIVE: Valid credentials should result in successful login
        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsTokenResponse_TruePositive()
        {
            // Arrange
            var testUser = CreateTestUser();
            _context.User.Add(testUser);
            await _context.SaveChangesAsync();

            var loginRequest = new UserDto
            {
                Username = "testuser",
                Password = "TestPassword123!"
            };

            // Act
            var result = await _authService.LoginAsync(loginRequest);

            // Assert - Should return successful login
            Assert.NotNull(result);
            Assert.NotNull(result.AccessToken);
            Assert.NotNull(result.RefreshToken);
            Assert.NotEmpty(result.AccessToken);
            Assert.NotEmpty(result.RefreshToken);

            // Verify refresh token was saved to database
            var updatedUser = await _context.User.FirstOrDefaultAsync(u => u.Username == "testuser");
            Assert.NotNull(updatedUser);
            Assert.NotNull(updatedUser.RefreshToken);
            Assert.True(updatedUser.RefreshTokenExpiryTime > DateTime.Now);
        }

        // TRUE NEGATIVE: Invalid credentials should be rejected
        [Fact]
        public async Task LoginAsync_UserNotFound_ReturnsNull_TrueNegative()
        {
            // Arrange - No user in database
            var loginRequest = new UserDto
            {
                Username = "nonexistentuser",
                Password = "TestPassword123!"
            };

            // Act
            var result = await _authService.LoginAsync(loginRequest);

            // Assert - Should correctly reject login
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_ReturnsNull_TrueNegative()
        {
            // Arrange
            var testUser = CreateTestUser();
            _context.User.Add(testUser);
            await _context.SaveChangesAsync();

            var loginRequest = new UserDto
            {
                Username = "testuser",
                Password = "WrongPassword123!"
            };

            // Act
            var result = await _authService.LoginAsync(loginRequest);

            // Assert - Should correctly reject login
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_EmptyPassword_ReturnsNull_TrueNegative()
        {
            // Arrange
            var testUser = CreateTestUser();
            _context.User.Add(testUser);
            await _context.SaveChangesAsync();

            var loginRequest = new UserDto
            {
                Username = "testuser",
                Password = ""
            };

            // Act
            var result = await _authService.LoginAsync(loginRequest);

            // Assert - Should correctly reject login
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_NullPassword_ReturnsNull_TrueNegative()
        {
            // Arrange
            var testUser = CreateTestUser();
            _context.User.Add(testUser);
            await _context.SaveChangesAsync();

            var loginRequest = new UserDto
            {
                Username = "testuser",
                Password = null!
            };

            // Act
            var result = await _authService.LoginAsync(loginRequest);

            // Assert - Should correctly reject login
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_CaseSensitiveUsername_ReturnsNull_TrueNegative()
        {
            // Arrange
            var testUser = CreateTestUser("testuser"); // lowercase
            _context.User.Add(testUser);
            await _context.SaveChangesAsync();

            var loginRequest = new UserDto
            {
                Username = "TESTUSER", // uppercase
                Password = "TestPassword123!"
            };

            // Act
            var result = await _authService.LoginAsync(loginRequest);

            // Assert - Should correctly reject login (assuming case-sensitive)
            Assert.Null(result);
        }


        #endregion

        #region RegisterAsync Tests

        // TRUE POSITIVE: Valid registration should create a new user
        [Fact]
        public async Task RegisterAsync_ValidRequest_ReturnsUser_TruePositive()
        {
            // Arrange
            var registerRequest = new RegisterUserDto
            {
                Username = "newuser",
                Password = "NewUserPassword123!",
                Email = "newuser@example.com",
                FirstName = "New",
                LastName = "User"
            };

            // Act
            var result = await _authService.RegisterAsync(registerRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(registerRequest.Username, result.Username);
            Assert.Equal(registerRequest.Email, result.Email);
            Assert.Equal(registerRequest.FirstName, result.FirstName);
            Assert.Equal(registerRequest.LastName, result.LastName);
        }

        // TRUE NEGATIVE: Duplicate username should return null
        [Fact]
        public async Task RegisterAsync_DuplicateUsername_ReturnsNull_TrueNegative()
        {
            // Arrange
            var existingUser = CreateTestUser("existinguser");
            _context.User.Add(existingUser);
            await _context.SaveChangesAsync();

            var registerRequest = new RegisterUserDto
            {
                Username = "existinguser", // Duplicate username
                Password = "NewUserPassword123!",
                Email = "existinguser@example.com",
                FirstName = "Existing",
                LastName = "User"
            };

            // Act
            var result = await _authService.RegisterAsync(registerRequest);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region RefreshTokensAsync Tests

        // TRUE POSITIVE: Valid refresh token should return new tokens
        [Fact]
        public async Task RefreshTokensAsync_ValidRequest_ReturnsTokenResponse_TruePositive()
        {
            // Arrange
            var testUser = CreateTestUser();
            _context.User.Add(testUser);
            await _context.SaveChangesAsync();

            // Simulate a valid refresh token
            testUser.RefreshToken = "valid-refresh-token";
            testUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();
            var refreshRequest = new RefreshTokenRequestDto
            {
                UserId = testUser.Id,
                RefreshToken = "valid-refresh-token"
            };
            // Act
            var result = await _authService.RefreshTokensAsync(refreshRequest);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.AccessToken);
            Assert.NotNull(result.RefreshToken);
            Assert.NotEmpty(result.AccessToken);
            Assert.NotEmpty(result.RefreshToken);
            Assert.NotEqual("valid-refresh-token", testUser.RefreshToken); // New refresh token generated
            Assert.True(testUser.RefreshTokenExpiryTime > DateTime.Now); // New expiry time
            Assert.NotEqual(testUser.RefreshTokenExpiryTime, DateTime.Now.AddDays(1)); // New expiry time

        }

        // TRUE NEGATIVE: Invalid refresh token should return null
        [Fact]
        public async Task RefreshTokensAsync_InvalidRequest_ReturnsNull_TrueNegative()
        {
            // Arrange
            var testUser = CreateTestUser();
            _context.User.Add(testUser);
            await _context.SaveChangesAsync();

            var refreshRequest = new RefreshTokenRequestDto
            {
                UserId = testUser.Id,
                RefreshToken = "invalid-refresh-token"
            };

            // Act
            var result = await _authService.RefreshTokensAsync(refreshRequest);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region AlterUserRoleAsync Tests

        // TRUE POSITIVE: Admin can change user role
        [Fact]
        public async Task AlterUserRoleAsync_ValidRequest_ReturnsUser_TruePositive()
        {
            // Arrange
            var testUser = CreateTestUser("testuser");
            _context.User.Add(testUser);
            await _context.SaveChangesAsync();

            var alterRoleRequest = new AlterUserRoleDto
            {
                Username = "testuser",
                NewRole = "Moderator"
            };

            // Act
            var result = await _authService.AlterUserRoleAsync(alterRoleRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Moderator", result.Role);
        }

        // TRUE NEGATIVE:
        // Non-admin user should not be able to change roles
        // tbd

        #endregion

        #region ChangePasswordAsync Tests

        // TRUE POSITIVE: Valid password change should succeed
        [Fact]
        public async Task ChangePasswordAsync_ValidRequest_ReturnsUser_TruePositive()
        {
            // Arrange
            var testUser = CreateTestUser();
            _context.User.Add(testUser);
            await _context.SaveChangesAsync();

            var changePasswordRequest = new ChangePasswordDto
            {
                Username = "testuser",
                OldPassword = "TestPassword123!",
                NewPassword = "NewPassword123!"
            };

            // Act
            var result = await _authService.ChangePasswordAsync(changePasswordRequest);

            // Assert
            Assert.NotNull(result);
            Assert.True(new PasswordHasher<User>().VerifyHashedPassword(result, result.PasswordHash, "NewPassword123!") == PasswordVerificationResult.Success);
        }

        #endregion



    }
}

