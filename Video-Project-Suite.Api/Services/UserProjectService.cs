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
            AssignedAt = DateOnly.FromDateTime(DateTime.UtcNow),
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

    public async Task<IEnumerable<UserProjectGetDto>> GetAllUserProjects()
    {
        // get all user projects, users, and projects, and map to list of UserProjectDto
        // this should be like an inner join
        var userProjects = await _context.UserProject
            .Include(up => up.Project)
            .Include(up => up.User)
            .ToListAsync();
        if (userProjects == null || !userProjects.Any())
        {
            return Enumerable.Empty<UserProjectGetDto>();
        }
        var userProjectDtos = userProjects.Select(up => new UserProjectGetDto
        {
            Id = up.Id,
            ProjectId = up.ProjectId,
            UserId = up.UserId,
            Role = up.Role,
            AssignedAt = up.AssignedAt,
            RemovedAt = up.RemovedAt,
            ProjectShortName = up.Project.ShortName,
            UserName = up.User.Username
        });

        return userProjectDtos;
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

    public async Task<UserProjectDto?> UpdateUserProject(int userProjectId, UserProjectDto userProjectDto)
    {
        var oldUserProject = await _context.UserProject.FindAsync(userProjectId);
        if (oldUserProject == null)
        {
            return null;
        }
        else
        {
            oldUserProject.ProjectId = userProjectDto.ProjectId;
            oldUserProject.UserId = userProjectDto.UserId;
            oldUserProject.Role = userProjectDto.Role;
            oldUserProject.AssignedAt = userProjectDto.AssignedAt;
            oldUserProject.RemovedAt = userProjectDto.RemovedAt;

            _context.UserProject.Update(oldUserProject);
            await _context.SaveChangesAsync();

            var newUserProjectUpdated = await _context.UserProject.FindAsync(userProjectId);
            if (newUserProjectUpdated == null)
            {
                return null;
            }

            var updatedUserProjectDto = new UserProjectDto
            {
                ProjectId = newUserProjectUpdated.ProjectId,
                UserId = newUserProjectUpdated.UserId,
                Role = newUserProjectUpdated.Role,
                AssignedAt = newUserProjectUpdated.AssignedAt,
                RemovedAt = newUserProjectUpdated.RemovedAt
            };

            return updatedUserProjectDto;
        }
    }
}
