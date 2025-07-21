using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Video_Project_Suite.Api.Services;

namespace Video_Project_Suite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController(IProjectService projectService) : ControllerBase
    {
        // Get all projects

        [HttpGet("projects")]
        public IActionResult GetAllProjects()
        {
            // need to make function async, removing for now to supress errors
            return Ok("get all projects");
        }

        // Get a project by ID
        [HttpGet("project/{projectId}")]
        public IActionResult GetProjectById(int projectId)
        {
            return Ok($"get project by id {projectId}");
        }

        // Get projects by user name

        [HttpGet("user/{username}/projects")]
        public IActionResult GetProjectsByUserName(string username)
        {
            return Ok($"get projects by user name {username}");
        }

        // Get Create a new project, returns users with roles

        [HttpGet("project/new")]
        public IActionResult GetCreateNewProject()
        {
            return Ok("get create new project");
        }


        // Post Create a new project
        [HttpPost("project/new")]
        public IActionResult CreateProject()
        {
            return Ok("create new project");
        }


        // Get Update an existing project, return current project data
        [HttpGet("project/{projectId}/edit")]
        public IActionResult GetUpdateProject(int projectId)
        {
            return Ok($"get update project {projectId}");
        }

        // Put Update an existing project
        [HttpPut("project/{projectId}")]
        public IActionResult UpdateProject(int projectId)
        {
            return Ok($"update project {projectId}");
        }

        // Delete a project by ID
        [HttpDelete("project/{projectId}")]
        public IActionResult DeleteProject(int projectId)
        {
            return Ok($"delete project {projectId}");
        }

    }
}
