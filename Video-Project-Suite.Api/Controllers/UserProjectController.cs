using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Video_Project_Suite.Api.Models.UserProject;
using Video_Project_Suite.Api.Services;

namespace Video_Project_Suite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProjectController(IUserProjectService userProjectService) : ControllerBase
    {
        // Get all

        [HttpGet("all-user-projects")]
        public async Task<IActionResult> GetAllUserProjects()
        {
            var userProjectsDtos = await userProjectService.GetAllUserProjects();
            return Ok(userProjectsDtos);
        }

        // Get by ID
        [HttpGet("{userProjectId}")]
        public async Task<IActionResult> GetUserProjectById(int userProjectId)
        {
            var userProject = await userProjectService.GetUserProjectById(userProjectId);
            return Ok(userProject);
        }


        // Get all by user name

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserProjectsByUserId(int userId)
        {
            var userProjects = await userProjectService.GetUserProjectsByUserId(userId);
            return Ok(userProjects);
        }

        // Get all by project name

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetUserProjectsByProjectId(int projectId)
        {
            var userProjects = await userProjectService.GetUserProjectsByProjectId(projectId);
            return Ok(userProjects);
        }

        // Create new user project

        [HttpPost("new-user-project")]
        public async Task<IActionResult> NewUserProject(UserProjectDto userProjectDto)
        {
            var newUserProjectDto = await userProjectService.CreateUserProject(userProjectDto);
            return Ok(newUserProjectDto);
        }

        // Update user project
        [HttpPut("{userProjectId}")]
        public async Task<IActionResult> UpdateUserProject(int userProjectId, UserProjectDto userProjectDto)
        {
            var updatedUserProject = await userProjectService.UpdateUserProject(userProjectId, userProjectDto);
            if (updatedUserProject == null)
            {
                return NotFound();
            }
            return Ok(updatedUserProject);
        }

        // Delete user project
        [HttpDelete("{userProjectId}")]
        public async Task<IActionResult> DeleteUserProject(int userProjectId)
        {
            var deletedUserProject = await userProjectService.DeleteUserProject(userProjectId);
            if (deletedUserProject == null)
            {
                return NotFound();
            }
            return Ok(deletedUserProject);
        }
    }
}
