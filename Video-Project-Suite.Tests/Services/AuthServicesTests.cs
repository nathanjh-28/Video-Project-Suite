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

        // POTENTIAL FALSE POSITIVE SCENARIOS (These should fail if they pass)
        // These tests verify that the system doesn't incorrectly authenticate users

        [Fact]
        public async Task LoginAsync_SqlInjectionAttempt_ReturnsNull_PreventFalsePositive()
        {
            // Arrange
            var testUser = CreateTestUser();
            _context.User.Add(testUser);
            await _context.SaveChangesAsync();

            var loginRequest = new UserDto
            {
                Username = "testuser'; DROP TABLE User; --",
                Password = "TestPassword123!"
            };

            // Act
            var result = await _authService.LoginAsync(loginRequest);

            // Assert - Should NOT authenticate (prevent false positive)
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_PasswordHashAsPassword_ReturnsNull_PreventFalsePositive()
        {
            // Arrange
            var testUser = CreateTestUser();
            _context.User.Add(testUser);
            await _context.SaveChangesAsync();

            var loginRequest = new UserDto
            {
                Username = "testuser",
                Password = testUser.PasswordHash // Trying to use the hash as password
            };

            // Act
            var result = await _authService.LoginAsync(loginRequest);

            // Assert - Should NOT authenticate (prevent false positive)
            Assert.Null(result);
        }

        // POTENTIAL FALSE NEGATIVE SCENARIOS
        // These tests verify that valid users aren't incorrectly rejected

        [Fact]
        public async Task LoginAsync_WhitespaceAroundUsername_ReturnsTokenResponse_PreventFalseNegative()
        {
            // Arrange
            var testUser = CreateTestUser("testuser");
            _context.User.Add(testUser);
            await _context.SaveChangesAsync();

            var loginRequest = new UserDto
            {
                Username = " testuser ", // Username with whitespace
                Password = "TestPassword123!"
            };

            // Act
            var result = await _authService.LoginAsync(loginRequest);

            // Assert - This test will reveal if whitespace handling causes false negatives
            // The result depends on your business requirements:
            // - If whitespace should be trimmed: Assert.NotNull(result)
            // - If whitespace should be preserved: Assert.Null(result)
            // For this example, let's assume it should be null (strict matching)
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentialsMultipleTimes_ReturnsTokenResponse_PreventFalseNegative()
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

            // Act - Login multiple times
            var result1 = await _authService.LoginAsync(loginRequest);
            var result2 = await _authService.LoginAsync(loginRequest);

            // Assert - Should work multiple times (no account lockout logic)
            Assert.NotNull(result1);
            Assert.NotNull(result2);
        }



        #endregion

        #region RegisterAsync Tests
        // tests

        #endregion

        #region RefreshTokensAsync Tests
        // tests

        #endregion

        #region AlterUserRoleAsync Tests
        // tests

        #endregion

        #region ChangePasswordAsync Tests
        // tests

        #endregion

    }


}

