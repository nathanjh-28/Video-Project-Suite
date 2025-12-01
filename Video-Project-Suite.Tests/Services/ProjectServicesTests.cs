using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        // Helper method to create one dummy projectDto
        private ProjectDto CreateProjectDto()
        {
            var temp_handle = Guid.NewGuid();
            return new ProjectDto
            {
                Title = $"Project Title {temp_handle:N}",
                ShortName = temp_handle.ToString("N").Substring(0, 6), // Short name is a substring of the GUID
                Focus = $"Focus Area {temp_handle:N}",
                Scope = $"Project Scope {temp_handle:N}",
                PricePerUnit = 10,
                QtyOfUnits = 5,
                ExpenseBudget = 1000,
                ExpenseSummary = "Expense Summary",
                Comments = "Comments",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                Type = "Project Type"
            };
        }

        // Helper method to convert Project to ProjectDto
        private ProjectDto ConvertToProjectDtoFromProject(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                ShortName = project.ShortName,
                Title = project.Title,
                Focus = project.Focus,
                Scope = project.Scope,
                PricePerUnit = project.PricePerUnit,
                QtyOfUnits = project.QtyOfUnits,
                ExpenseBudget = project.ExpenseBudget,
                ExpenseSummary = project.ExpenseSummary,
                Comments = project.Comments,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Type = project.Type
            };
        }

        // Helper method to convert ProjectDto to Project
        private Project ConvertToProjectFromProjectDto(ProjectDto projectDto)
        {
            return new Project
            {
                Id = projectDto.Id,
                ShortName = projectDto.ShortName,
                Title = projectDto.Title,
                Focus = projectDto.Focus,
                Scope = projectDto.Scope,
                PricePerUnit = projectDto.PricePerUnit,
                QtyOfUnits = projectDto.QtyOfUnits,
                ExpenseBudget = projectDto.ExpenseBudget,
                ExpenseSummary = projectDto.ExpenseSummary,
                Comments = projectDto.Comments,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate,
                Type = projectDto.Type
            };
        }


        // Test methods for ProjectService

        #region GetAllProjectsAsync

        // True Positive Test: Retrieve all projects when projects exist in db
        [Fact]
        public async Task GetAllProjectsAsync_ShouldReturnAllProjects_WhenProjectsExist()
        {
            // Arrange

            // This will have to be updated as I alter required properties in ProjectDto

            var project1 = ConvertToProjectFromProjectDto(CreateProjectDto());
            var project2 = ConvertToProjectFromProjectDto(CreateProjectDto());
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
            var newProjectDto = CreateProjectDto();

            // Act
            var projectDto = await _projectService.CreateProjectAsync(newProjectDto);

            // Assert
            Assert.NotNull(projectDto);
            Assert.Equal(newProjectDto.ShortName, projectDto.ShortName);
            Assert.Equal(newProjectDto.Focus, projectDto.Focus);
            Assert.Equal(newProjectDto.Scope, projectDto.Scope);
            Assert.Equal(newProjectDto.PricePerUnit, projectDto.PricePerUnit);
            Assert.Equal(newProjectDto.QtyOfUnits, projectDto.QtyOfUnits);
            Assert.Equal(newProjectDto.ExpenseBudget, projectDto.ExpenseBudget);
            Assert.Equal(newProjectDto.ExpenseSummary, projectDto.ExpenseSummary);
            Assert.Equal(newProjectDto.Comments, projectDto.Comments);
            Assert.Equal(newProjectDto.StartDate, projectDto.StartDate);
            Assert.Equal(newProjectDto.EndDate, projectDto.EndDate);
            Assert.True(projectDto.StartDate < projectDto.EndDate, "Start date should be before end date");
            Assert.Equal(newProjectDto.Type, projectDto.Type);
            Assert.Equal(newProjectDto.Title, projectDto.Title);
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
            var project = ConvertToProjectFromProjectDto(CreateProjectDto());
            project.Id = 11; // Set a specific ID for the project
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
            var project = ConvertToProjectFromProjectDto(CreateProjectDto());
            project.Id = 111; // Set a specific ID for the project
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
            var updateProjectDto = CreateProjectDto();

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
            var project = ConvertToProjectFromProjectDto(CreateProjectDto());
            project.Id = 222; // Set a specific ID for the project
            _context.Project.Add(project);
            _context.SaveChanges();

            var updateProjectDto = CreateProjectDto();

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


    }

}

