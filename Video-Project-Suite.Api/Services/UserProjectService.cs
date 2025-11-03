using System;
using Microsoft.EntityFrameworkCore;
using Video_Project_Suite.Api.Data;
using Video_Project_Suite.Api.Models.UserProject;

namespace Video_Project_Suite.Api.Services;

public class UserProjectService : IUserProjectService
{
    private readonly AppDbContext _context;

    public UserProjectService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProjectDto> CreateUserProject(UserProjectDto userProjectDto)
    {
        var newUserProject = new Models.UserProject.UserProject
        {
            ProjectId = userProjectDto.ProjectId,
            UserId = userProjectDto.UserId,
            AssignedAt = DateTime.UtcNow,
            Role = userProjectDto.Role
        };

        var entry = _context.UserProject.Add(newUserProject);
        await _context.SaveChangesAsync();

        if (entry.Entity.ProjectId <= 0 || entry.Entity.UserId <= 0)
        {
            throw new Exception("UserProject was not created successfully");
        }

        var newUserProjectDto = new UserProjectDto
        {
            ProjectId = entry.Entity.ProjectId,
            UserId = entry.Entity.UserId,
            Role = entry.Entity.Role
        };

        return newUserProjectDto;


    }

    public async Task<UserProject?> DeleteUserProject(int userProjectId)
    {
        var userProject = await _context.UserProject.FindAsync(userProjectId);
        if (userProject == null)
        {
            return null;
        }
        _context.UserProject.Remove(userProject);
        await _context.SaveChangesAsync();
        return userProject;
    }

    public async Task<IEnumerable<UserProject>> GetAllUserProjects()
    {
        var userProjects = await _context.UserProject.ToListAsync();
        if (userProjects == null || !userProjects.Any())
        {
            return Enumerable.Empty<UserProject>();
        }
        return userProjects;
    }

    public async Task<UserProject> GetUserProjectById(int userProjectId)
    {
        var userProject = await _context.UserProject.FindAsync(userProjectId);
        if (userProject == null)
        {
            throw new Exception("UserProject not found");
        }
        return userProject;
    }

    public async Task<IEnumerable<UserProject>> GetUserProjectsByProjectId(int projectId)
    {
        var userProjects = await _context.UserProject
            .Where(up => up.ProjectId == projectId)
            .ToListAsync();
        return userProjects;
    }

    public async Task<IEnumerable<UserProject>> GetUserProjectsByUserId(int userId)
    {
        var userProjects = await _context.UserProject
            .Where(up => up.UserId == userId)
            .ToListAsync();
        return userProjects;
    }

    public async Task<UserProject?> UpdateUserProject(int userProjectId, UserProject userProject)
    {
        var oldUserProject = await _context.UserProject.FindAsync(userProjectId);
        if (oldUserProject == null)
        {
            return null;
        }
        else
        {
            oldUserProject.RemovedAt = userProject.RemovedAt;
            oldUserProject.Role = userProject.Role;
            await _context.SaveChangesAsync();
            return oldUserProject;
        }
    }
}
