using System;
using Video_Project_Suite.Api.Models;

namespace Video_Project_Suite.Api.Services;

public interface IProjectService
{
    // Get all projects
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();

    // Get project by ID
    Task<ProjectDto?> GetProjectByIdAsync(int projectId);

    // Create a new project
    Task<ProjectDto?> CreateProjectAsync(ProjectDto newProjectDto);

    // Update an existing project
    Task<ProjectDto?> UpdateProjectAsync(int projectId, ProjectDto projectDto);

    // Delete a project
    Task<ProjectDto?> DeleteProjectAsync(int projectId);

    // update project milestone
    Task<bool> UpdateProjectMilestoneAsync(int projectId, int milestoneId);

}
