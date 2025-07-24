using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Services;

namespace Video_Project_Suite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController(IProjectService projectService) : ControllerBase
    {
        // Get all projects

        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        // Get a project by ID
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            var project = await projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        // Get projects by user name

        [HttpGet("user/{username}/projects")]
        public IActionResult GetProjectsByUserName(string username)
        {
            return Ok($"get projects by user name {username}");
        }

        // Get Create a new project, returns users with roles

        [HttpGet("project/new")]
        public async Task<ActionResult<ProjectDto>> GetCreateNewProject()
        {
            // This method should return a view or a DTO for creating a new project
            // For now, we will just return an empty ProjectDto
            var newProjectDto = new ProjectDto();
            return Ok(newProjectDto);
        }

        // Post Create a new project
        [HttpPost("project/new")]
        public async Task<IActionResult> CreateProject(ProjectDto newProjectDto)
        {
            if (newProjectDto == null)
            {
                return BadRequest("Project data is required.");
            }

            var createdProject = await projectService.CreateProjectAsync(newProjectDto);
            if (createdProject == null)
            {
                return BadRequest("Failed to create project.");
            }
            return Ok(createdProject);
            // can't use this because the dto does not have the id, need helper method for
            // getting an id with short name
            // return CreatedAtAction(nameof(GetProjectById), new { projectShortName = createdProject.ShortName }, createdProject);
        }


        // Get Update an existing project, return current project data
        [HttpGet("project/{projectId}/update")]
        public async Task<IActionResult> GetUpdateProject(int projectId)
        {
            var updated_project = await projectService.GetProjectByIdAsync(projectId);
            if (updated_project == null)
            {
                return NotFound();
            }
            return Ok(updated_project);
        }

        // Put Update an existing project
        [HttpPut("project/{projectId}/update")]
        public async Task<IActionResult> UpdateProject(int projectId, ProjectDto projectDto)
        {
            if (projectDto == null)
            {
                return BadRequest("Project data is required.");
            }

            var updatedProject = await projectService.UpdateProjectAsync(projectId, projectDto);
            if (updatedProject == null)
            {
                return NotFound();
            }
            return Ok(updatedProject);
        }


        // Delete a project by ID
        [HttpDelete("project/{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            var result = await projectService.DeleteProjectAsync(projectId);
            if (result == null)
            {
                return NotFound();
            }
            return NoContent(); // Return 204 No Content on successful deletion
        }

    }
}
