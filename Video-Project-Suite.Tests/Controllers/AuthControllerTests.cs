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
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Video_Project_Suite.Api.Data;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Models.Dto;
using Xunit;

namespace Video_Project_Suite.Tests.Controllers
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly string _baseUrl = "/api/auth";

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration
                    services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
                    services.RemoveAll(typeof(AppDbContext));

                    // Add in-memory database for testing
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}");
                    });

                    // Ensure the database is created
                    var serviceProvider = services.BuildServiceProvider();
                    using var scope = serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    context.Database.EnsureCreated();
                });
            });

            _client = _factory.CreateClient();
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

        public void Dispose()
        {
            _client.Dispose();
        }

        /* Testing is not working yet for this file.

        #region RegisterAsync Tests

        [Fact]
        public async Task Register_ValidUser_ReturnsOkWithUser()
        {
            // Arrange:
            // - Create a RegisterUserDto with valid data (username, password, email)
            var request = new RegisterUserDto
            {
                Username = "testuser",
                Email = "testuser@email.com",
                Password = "StrongPassword123",
                FirstName = "Test",
                LastName = "User"
            };
            // - Serialize it to JSON content
            var content = CreateJsonContent(request);

            // Act:
            // - POST to /api/auth/register with the JSON content
            var response = await _client.PostAsync($"{_baseUrl}/register", content);
            // - Get the response
            var result = await DeserializeResponse<User>(response);

            // Assert:
            // - Response status code should be 200 OK
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // - Response should contain a User object
            Assert.NotNull(result);
            // - User object should have matching username and email
            Assert.Equal(request.Username, result.Username);
            Assert.Equal(request.Email, result.Email);
            // - User object should not contain password
            Assert.Null(result.PasswordHash);
            // - User object should have FirstName and LastName set
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);

        }

        [Fact]
        public async Task Register_InvalidUser_ReturnsBadRequest()
        {
            // Not implemented yet


            // Arrange:
            // - Create a RegisterUserDto with invalid data (empty username, weak password, invalid email)
            // - Serialize it to JSON content

            // Act:
            // - POST to /api/auth/register with the invalid JSON content
            // - Get the response

            // Assert:
            // - Response status code should be 400 Bad Request
            // - Response should contain error message
        }

        [Fact]
        public async Task Register_DuplicateUser_ReturnsBadRequest()
        {
            // Arrange:
            // - Create a RegisterUserDto with valid data
            // - Register the user once (should succeed)
            // - Try to register the same user again

            // Act:
            // - POST to /api/auth/register with the same user data twice
            // - Get the second response

            // Assert:
            // - First registration should return 200 OK
            // - Second registration should return 400 Bad Request
            // - Second response should contain appropriate error message
        }

        #endregion

        */
        #region LoginAsync Tests

        // tests

        #endregion

        #region ChangePassword Tests

        // tests

        #endregion

        #region AlterUserRoleAsync Tests

        // tests

        #endregion

        #region RefreshTokensAsync Tests

        // tests

        #endregion

    }



}

