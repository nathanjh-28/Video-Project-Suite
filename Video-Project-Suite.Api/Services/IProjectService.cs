using System;
using Video_Project_Suite.Api.Models;

namespace Video_Project_Suite.Api.Services;

public interface IProjectService
{
    // Get all projects
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();

    // Get all projects by username
    Task<IEnumerable<ProjectDto>> GetProjectsByUserNameAsync(string username);

    // Get project by ID
    Task<ProjectDto> GetProjectByIdAsync(int projectId);

    // helper method - get all users with roles of "x"
    Task<IEnumerable<string>> GetUsersWithRoleAsync(string role);

    // Create a new project
    Task<ProjectDto> CreateProjectAsync(ProjectDto newProjectDto);

    // Update an existing project
    Task<ProjectDto> UpdateProjectAsync(int projectId, ProjectDto projectDto);

    // Delete a project
    Task<bool> DeleteProjectAsync(int projectId);

}
