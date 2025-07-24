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
using Video_Project_Suite.Api.Models.Project;
using Video_Project_Suite.Api.Services;
using Xunit;


namespace Video_Project_Suite.Api.Tests.Services
{
    public class ProjectServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        // private readonly IConfiguration _configuration;
        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            // Setup in-memory database for testing
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Setup configuration mock
            // _configuration = CreateConfiguration();


            _projectService = new ProjectService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // Test methods for ProjectService

        #region GetAllProjectsAsync

        // True Positive Test: Retrieve all projects when projects exist in db
        [Fact]
        public async Task GetAllProjectsAsync_ShouldReturnAllProjects_WhenProjectsExist()
        {
            // Arrange

            // This will have to be updated as I alter required properties in ProjectDto

            var project1 = new Project { Id = 1, ShortName = "Project 1" };
            var project2 = new Project { Id = 2, ShortName = "Project 2" };
            _context.Project.AddRange(project1, project2);
            _context.SaveChanges();

            // Act
            var result = await _projectService.GetAllProjectsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        // True Negative Test: Retrieve all projects when no projects exist in db
        [Fact]
        public async Task GetAllProjectsAsync_ShouldReturnEmptyList_WhenNoProjectsExist()
        {
            // Arrange
            // No projects added to the context

            // Act
            var result = await _projectService.GetAllProjectsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }


        #endregion

        #region CreateProjectAsync

        // True Negative Test: Attempt to create a project with invalid data
        [Fact]
        public async Task CreateProjectAsync_ShouldReturnNull_WhenInvalidData()
        {
            // Arrange
            var newProjectDto = new ProjectDto
            {
                ShortName = "" // Invalid data: empty short name
            };

            // Act
            var projectDto = await _projectService.CreateProjectAsync(newProjectDto);

            // Assert
            Assert.Null(projectDto);
        }


        // True Positive Test: Create a new project successfully
        [Fact]
        public async Task CreateProjectAsync_ShouldReturnProject_WhenValidData()
        {
            // Arrange
            var newProjectDto = new ProjectDto
            {
                ShortName = "New Project",
                Focus = "Focus Area",
                Scope = "Project Scope",
                PricePerUnit = 100.0m,
                QtyOfUnits = 10,
                ExpenseBudget = 1000.0m,
                ExpenseSummary = "Expense Summary",
                Comments = "Project Comments",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1))
            };

            // Act
            var projectDto = await _projectService.CreateProjectAsync(newProjectDto);

            // Assert
            Assert.NotNull(projectDto);
            Assert.Equal("New Project", projectDto.ShortName);
            Assert.Equal("Focus Area", projectDto.Focus);
            Assert.Equal("Project Scope", projectDto.Scope);
            Assert.Equal(100.0m, projectDto.PricePerUnit);
            Assert.Equal(10, projectDto.QtyOfUnits);
            Assert.Equal(1000.0m, projectDto.ExpenseBudget);
            Assert.Equal("Expense Summary", projectDto.ExpenseSummary);
            Assert.Equal("Project Comments", projectDto.Comments);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now), projectDto.StartDate);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now.AddMonths(1)), projectDto.EndDate);
            Assert.True(projectDto.StartDate < projectDto.EndDate, "Start date should be before end date");
        }

        #endregion

        #region GetProjectByIdAsync
        // True Negative Test: Retrieve a project by ID when it does not exist
        [Fact]
        public async Task GetProjectByIdAsync_ShouldReturnNull_WhenProjectDoesNotExist()
        {
            // Arrange

            // What if there are no porjects in the database?



            int nonExistentProjectId = 2147483647; // This is the largest possible value for an int, assuming no project with this ID exists in the database.

            // Act
            var result = await _projectService.GetProjectByIdAsync(nonExistentProjectId);

            // Assert
            Assert.Null(result);
        }

        // True Positive Test: Retrieve a project by ID when it exists

        [Fact]
        public async Task GetProjectByIdAsync_ShouldReturnProject_WhenProjectExists()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                ShortName = "Existing Project",
                Focus = "Focus Area",
                Scope = "Project Scope",
                PricePerUnit = 100.0m,
                QtyOfUnits = 10,
                ExpenseBudget = 1000.0m,
                ExpenseSummary = "Expense Summary",
                Comments = "Project Comments",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1))
            };
            _context.Project.Add(project);
            _context.SaveChanges();

            // Act
            var result = await _projectService.GetProjectByIdAsync(project.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project.ShortName, result.ShortName);
            Assert.Equal(project.Focus, result.Focus);
            Assert.Equal(project.Scope, result.Scope);
            Assert.Equal(project.PricePerUnit, result.PricePerUnit);
            Assert.Equal(project.QtyOfUnits, result.QtyOfUnits);
            Assert.Equal(project.ExpenseBudget, result.ExpenseBudget);
            Assert.Equal(project.ExpenseSummary, result.ExpenseSummary);
            Assert.Equal(project.Comments, result.Comments);
            Assert.Equal(project.StartDate, result.StartDate);
            Assert.Equal(project.EndDate, result.EndDate);
            Assert.True(result.StartDate < result.EndDate, "Start date should be before end date");

        }




        #endregion

        #region DeleteProjectAsync

        // True Negative Test: Attempt to delete a project that does not exist
        [Fact]
        public async Task DeleteProjectAsync_ShouldReturnNull_WhenProjectDoesNotExist()
        {
            // Arrange
            int nonExistentProjectId = 2147483647; // This is the largest possible value for an int, assuming no project with this ID exists in the database.

            // Act
            var result = await _projectService.DeleteProjectAsync(nonExistentProjectId);

            // Assert
            Assert.Null(result);

        }

        // True Positive Test: Delete a project successfully
        [Fact]
        public async Task DeleteProjectAsync_ShouldReturnDeletedProject_WhenProjectExists()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                ShortName = "Project to Delete",
                Focus = "Focus Area",
                Scope = "Project Scope",
                PricePerUnit = 100.0m,
                QtyOfUnits = 10,
                ExpenseBudget = 1000.0m,
                ExpenseSummary = "Expense Summary",
                Comments = "Project Comments",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1))
            };
            _context.Project.Add(project);
            _context.SaveChanges();

            // Act
            var result = await _projectService.DeleteProjectAsync(project.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project.ShortName, result.ShortName);
            Assert.Equal(project.Focus, result.Focus);
            Assert.Equal(project.Scope, result.Scope);
            Assert.Equal(project.PricePerUnit, result.PricePerUnit);
            Assert.Equal(project.QtyOfUnits, result.QtyOfUnits);
            Assert.Equal(project.ExpenseBudget, result.ExpenseBudget);
            Assert.Equal(project.ExpenseSummary, result.ExpenseSummary);
            Assert.Equal(project.Comments, result.Comments);
        }


        #endregion

        #region UpdateProjectAsync
        // True Negative Test: Attempt to update a project that does not exist
        [Fact]
        public async Task UpdateProjectAsync_ShouldReturnNull_WhenProjectDoesNotExist()
        {
            // Arrange
            int nonExistentProjectId = 2147483647; // This is the largest possible value for an int, assuming no project with this ID exists in the database.
            var updateProjectDto = new ProjectDto
            {
                ShortName = "Updated Project",
                Focus = "Updated Focus",
                Scope = "Updated Scope",
                PricePerUnit = 200.0m,
                QtyOfUnits = 20,
                ExpenseBudget = 2000.0m,
                ExpenseSummary = "Updated Expense Summary",
                Comments = "Updated Comments",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(2))
            };

            // Act
            var result = await _projectService.UpdateProjectAsync(nonExistentProjectId, updateProjectDto);

            // Assert
            Assert.Null(result);
        }


        // True Positive Test: Update a project successfully

        [Fact]
        public async Task UpdateProjectAsync_ShouldReturnUpdatedProject_WhenProjectExists()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                ShortName = "Project to Update",
                Focus = "Focus Area",
                Scope = "Project Scope",
                PricePerUnit = 100.0m,
                QtyOfUnits = 10,
                ExpenseBudget = 1000.0m,
                ExpenseSummary = "Expense Summary",
                Comments = "Project Comments",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1))
            };
            _context.Project.Add(project);
            _context.SaveChanges();

            var updateProjectDto = new ProjectDto
            {
                ShortName = "Updated Project",
                Focus = "Updated Focus",
                Scope = "Updated Scope",
                PricePerUnit = 200.0m,
                QtyOfUnits = 20,
                ExpenseBudget = 2000.0m,
                ExpenseSummary = "Updated Expense Summary",
                Comments = "Updated Comments",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(2))
            };

            // Act
            var result = await _projectService.UpdateProjectAsync(project.Id, updateProjectDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateProjectDto.ShortName, result.ShortName);
            Assert.Equal(updateProjectDto.Focus, result.Focus);
            Assert.Equal(updateProjectDto.Scope, result.Scope);
            Assert.Equal(updateProjectDto.PricePerUnit, result.PricePerUnit);
            Assert.Equal(updateProjectDto.QtyOfUnits, result.QtyOfUnits);
            Assert.Equal(updateProjectDto.ExpenseBudget, result.ExpenseBudget);
            Assert.Equal(updateProjectDto.ExpenseSummary, result.ExpenseSummary);
            Assert.Equal(updateProjectDto.Comments, result.Comments);
        }


        #endregion

        #region GetProjectsByUserNameAsync
        // True Negative Test: Retrieve projects by username when no projects exist for that user

        // True Positive Test: Retrieve projects by username when projects exist for that user

        #endregion

        #region GetUsersWithRoleAsync
        // True Negative Test: Retrieve users with a role when no users exist with that role

        // True Positive Test: Retrieve users with a role when users exist with that role

        #endregion

    }

}

