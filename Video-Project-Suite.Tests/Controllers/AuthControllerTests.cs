// AuthControllerTests.cs

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Video_Project_Suite.Api;
using Video_Project_Suite.Api.Data;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Models.Dto;
using Xunit;

namespace Video_Project_Suite.Tests.Controllers
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _baseUrl = "/api/auth";

        private HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            var dbName = $"TestDb_{Guid.NewGuid()}";
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing AppDbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }
                    // Add the in-memory database for testing
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(dbName);
                    });
                });
            });
            _client = _factory.CreateClient();
        }

        private async Task ClearDatabaseAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Clear the database
            context.User.RemoveRange(context.User);
            context.Project.RemoveRange(context.Project);
            await context.SaveChangesAsync();
        }


        private static StringContent CreateJsonContent(object obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content)) return default;

            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private void Dispose()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        #region RegisterAsync Tests
        [Fact]
        public async Task Register_ValidUser_ReturnsOkWithUser()
        {
            // Arrange
            await ClearDatabaseAsync();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var request = new RegisterUserDto
            {
                Username = $"testuser_{Guid.NewGuid():N}",
                Email = $"testuser_{Guid.NewGuid():N}@email.com",
                Password = "StrongPassword123",
                FirstName = "Test",
                LastName = "User"
            };
            var content = CreateJsonContent(request);

            // Act
            var response = await _client.PostAsync($"{_baseUrl}/register", content);

            // Debug: Print the actual response
            var responseContent = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine($"Status Code: {response.StatusCode}");
            System.Console.WriteLine($"Response Content: {responseContent}");
            System.Console.WriteLine($"Response Headers: {response.Headers}");

            // Only try to deserialize if we have a successful response
            if (response.IsSuccessStatusCode)
            {
                var result = await DeserializeResponse<User>(response);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(result);
                Assert.Equal(request.Username, result.Username);
                Assert.Equal(request.Email, result.Email);
                Assert.Equal(request.FirstName, result.FirstName);
                Assert.Equal(request.LastName, result.LastName);
                // Assert.Null(result.PasswordHash);
            }
            else
            {
                // If not successful, let's see what we got
                Assert.Fail($"Expected success but got {response.StatusCode}: {responseContent}");
            }
        }


        [Fact]
        public async Task Register_DuplicateUser_ReturnsError()
        {
            // Arrange - Use same unique data for both requests
            await ClearDatabaseAsync();
            var request = new RegisterUserDto
            {
                Username = "duplicate_user",
                Email = "duplicate_user@email.com",
                Password = "StrongPassword123",
                FirstName = "Test",
                LastName = "User"
            };
            var content1 = CreateJsonContent(request);
            var content2 = CreateJsonContent(request);

            // Act - Register first time (should succeed)
            var firstResponse = await _client.PostAsync($"{_baseUrl}/register", content1);

            // debug statements
            // After first registration, check if user exists in database
            // using var checkScope = _factory.Services.CreateScope();
            // var checkContext = checkScope.ServiceProvider.GetRequiredService<AppDbContext>();
            // var userCount = await checkContext.User.CountAsync();
            // var existingUser = await checkContext.User.FirstOrDefaultAsync(u => u.Username == request.Username);
            // System.Console.WriteLine($"Users in database: {userCount}");
            // System.Console.WriteLine($"User exists: {existingUser != null}");

            // Act - Register second time (should fail)
            var secondResponse = await _client.PostAsync($"{_baseUrl}/register", content2);

            // Debug: Print the actual response
            var firstResponseContent = await firstResponse.Content.ReadAsStringAsync();
            var secondResponseContent = await secondResponse.Content.ReadAsStringAsync();
            System.Console.WriteLine($"First Response Status Code: {firstResponse.StatusCode}");
            System.Console.WriteLine($"First Response Content: {firstResponseContent}");
            System.Console.WriteLine($"Second Response Status Code: {secondResponse.StatusCode}");
            System.Console.WriteLine($"Second Response Content: {secondResponseContent}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
            Assert.NotEqual(HttpStatusCode.OK, secondResponse.StatusCode); // Should be 400/409/422 etc.
        }



        #endregion


        #region LoginAsync Tests

        // Register user then login with correct credentials
        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange - First register a user
            await ClearDatabaseAsync();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // var uniqueId = Guid.NewGuid().ToString("N")[..8];
            var registerRequest = new RegisterUserDto
            {
                Username = "testuser",
                Email = "testuser@email.com",
                Password = "StrongPassword123",
                FirstName = "Test",
                LastName = "User"
            };
            var registerContent = CreateJsonContent(registerRequest);

            // Register the user first
            var registerResponse = await _client.PostAsync($"{_baseUrl}/register", registerContent);
            registerResponse.EnsureSuccessStatusCode();

            // Arrange - Now prepare login request
            var loginRequest = new UserDto
            {
                Username = registerRequest.Username,
                Password = registerRequest.Password
            };
            var loginContent = CreateJsonContent(loginRequest);

            // Act - Login with the registered user credentials
            var loginResponse = await _client.PostAsync($"{_baseUrl}/login", loginContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var tokenResponse = await DeserializeResponse<TokenResponseDto>(loginResponse);
            Assert.NotNull(tokenResponse);
            Assert.NotNull(tokenResponse.AccessToken);
            Assert.NotEmpty(tokenResponse.AccessToken);
            Assert.NotNull(tokenResponse.RefreshToken);
            Assert.NotEmpty(tokenResponse.RefreshToken);

        }

        // Login with username that doesn't exist
        [Fact]
        public async Task Login_InvalidUsername_ReturnsBadRequest()
        {
            // Arrange - Create login request with username that doesn't exist
            await ClearDatabaseAsync();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var loginRequest = new UserDto
            {
                Username = $"nonexistent_user_{Guid.NewGuid().ToString("N")[..8]}",
                Password = "SomePassword123"
            };
            var loginContent = CreateJsonContent(loginRequest);

            // Act - Try to login with non-existent username
            var response = await _client.PostAsync($"{_baseUrl}/login", loginContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid username or password", responseContent);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsBadRequest()
        {

            // Login with existing username but wrong password


            await ClearDatabaseAsync();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // Arrange - First register a user
            var uniqueId = Guid.NewGuid().ToString("N")[..8];
            var registerRequest = new RegisterUserDto
            {
                Username = $"testuser_{uniqueId}",
                Email = $"testuser_{uniqueId}@email.com",
                Password = "CorrectPassword123",
                FirstName = "Test",
                LastName = "User"
            };
            var registerContent = CreateJsonContent(registerRequest);

            // Register the user first
            var registerResponse = await _client.PostAsync($"{_baseUrl}/register", registerContent);
            registerResponse.EnsureSuccessStatusCode();

            // Arrange - Now prepare login request with wrong password
            var loginRequest = new UserDto
            {
                Username = registerRequest.Username, // Correct username
                Password = "WrongPassword456"        // Wrong password
            };
            var loginContent = CreateJsonContent(loginRequest);

            // Act - Try to login with correct username but wrong password
            var response = await _client.PostAsync($"{_baseUrl}/login", loginContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid username or password", responseContent);
        }

        #endregion


        // TBD more auth tests...


        #region ChangePassword Tests
        // [Fact]
        // public async Task ChangePassword_WithoutAuth_ReturnsUnauthorized()
        // {
        //     // Try to change password without being logged in
        //     // TODO: Implement
        // }
        #endregion

        #region AlterUserRoleAsync Tests

        // tbd

        #endregion

        #region RefreshTokensAsync Tests

        // REFRESH TOKEN TESTS
        // [Fact]
        // public async Task RefreshToken_ValidToken_ReturnsNewTokens()
        // {
        //     // Login and use refresh token to get new access token
        //     // TODO: Implement
        // }

        // [Fact]
        // public async Task RefreshToken_InvalidToken_ReturnsUnauthorized()
        // {
        //     // Try to refresh with invalid/expired refresh token
        //     // TODO: Implement
        // }

        #endregion

        [Fact]
        public async Task Diagnostic_SameDbContextAcrossRequests()
        {
            // Clear and add a user directly to the database
            await ClearDatabaseAsync();

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.User.Add(new User
                {
                    Username = "diagnostic_user",
                    Email = "diagnostic@test.com",
                    // ... other required fields
                });
                await context.SaveChangesAsync();
            }

            // Now make an HTTP request that should see this user
            var loginRequest = new UserDto
            {
                Username = "diagnostic_user",
                Password = "any_password"
            };
            var content = CreateJsonContent(loginRequest);
            var response = await _client.PostAsync($"{_baseUrl}/login", content);

            // Check if the controller can see the user we just added
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userCount = await context.User.CountAsync();
                System.Console.WriteLine($"Users in database: {userCount}");
            }
        }

    }



}