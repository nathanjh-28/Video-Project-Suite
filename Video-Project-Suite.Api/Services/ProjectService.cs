using System;
using Video_Project_Suite.Api.Data;
using Video_Project_Suite.Api.Models;

namespace Video_Project_Suite.Api.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;

    public ProjectService(AppDbContext context)
    {
        _context = context;
    }

    public Task<ProjectDto> CreateProjectAsync(ProjectDto newProjectDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteProjectAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ProjectDto> GetProjectByIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProjectDto>> GetProjectsByUserNameAsync(string username)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<string>> GetUsersWithRoleAsync(string role)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectDto> UpdateProjectAsync(int projectId, ProjectDto projectDto)
    {
        throw new NotImplementedException();
    }
}
