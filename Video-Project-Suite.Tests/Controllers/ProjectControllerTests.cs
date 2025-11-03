// ProjectControllerTests.cs

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
using Video_Project_Suite.Api.Models.Project;
using Video_Project_Suite.Api.Models.User;
using Xunit;

namespace Video_Project_Suite.Tests.Controllers
{
    public class ProjectControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _baseUrl = "/api/project";

        private HttpClient _client;

        public ProjectControllerTests(WebApplicationFactory<Program> factory)
        {
            // how do we set up the in memory test sqlite database?
            // This is a workaround to use the same factory for all tests

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

        [Fact]
        public void Dispose()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        #region GetAllProjects


        // True Negative Test: Retrieve all projects when no projects exist in db
        [Fact]
        public async Task GetAllProjects_ReturnsOk_WhenNoProjectsExist()
        {
            // Arrange
            // How do you simulate no projects in the database?
            // The in-memory database is already empty by default, so we don't need to do anything
            await ClearDatabaseAsync();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Act
            var response = await _client.GetAsync(_baseUrl + "/projects");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var projects = await DeserializeResponse<List<ProjectDto>>(response);
            Assert.NotNull(projects);
            Assert.Empty(projects);
        }

        // True Positive Test: Retrieve all projects when projects exist in db

        [Fact]
        public async Task GetAllProjects_ReturnsOk_WhenProjectsExist()
        {
            // Arrange
            await ClearDatabaseAsync();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var project1 = new ProjectDto { ShortName = "Project 1" };
            var project2 = new ProjectDto { ShortName = "Project 2" };
            var projectDtos = new List<ProjectDto> { project1, project2 };
            var jsonContent = CreateJsonContent(projectDtos);

            // convert dtos to projects
            var projects = new List<Project>
                    {
                        new Project
                        {
                            ShortName = project1.ShortName },
                        new Project
                        {
                            ShortName = project2.ShortName }
                    };

            context.Project.AddRange(projects);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync(_baseUrl + "/projects");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(jsonContent.Headers.ContentType, response.Content.Headers.ContentType);
        }


        #endregion

    }



}