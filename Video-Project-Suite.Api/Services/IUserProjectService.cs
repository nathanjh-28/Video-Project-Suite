using System;
using Video_Project_Suite.Api.Models.UserProject;

namespace Video_Project_Suite.Api.Services;

public interface IUserProjectService
{
    // Get all User Projects
    Task<IEnumerable<UserProject>> GetAllUserProjects();

    // Get by UserProject ID
    Task<UserProject> GetUserProjectById(int userProjectId);

    // Get all by User ID
    Task<IEnumerable<UserProject>> GetUserProjectsByUserId(int userId);

    // Get all by Project ID
    Task<IEnumerable<UserProject>> GetUserProjectsByProjectId(int projectId);

    // Create new UserProject
    Task<UserProjectDto> CreateUserProject(UserProjectDto userProjectDto);

    // Update UserProject
    Task<UserProject?> UpdateUserProject(int userProjectId, UserProject userProject);

    // Delete UserProject
    Task<UserProject?> DeleteUserProject(int userProjectId);
}
