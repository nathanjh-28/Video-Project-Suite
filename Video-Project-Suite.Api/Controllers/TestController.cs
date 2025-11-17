// Controllers/TestController.cs

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Video_Project_Suite.Api.Controllers
{
    // simple test route to verify the API is working

    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Hello from .NET API!", timestamp = DateTime.Now });
        }
        // simple test route to verify that authentication is working

        [Authorize]
        [HttpGet("secure")]
        public IActionResult GetSecure()
        {
            return Ok(new { message = "Hello from secure .NET API!", timestamp = DateTime.Now });
        }

        // simple test route to verify authorization is working
        [Authorize(Roles = "admin")]
        [HttpGet("admin")]
        public IActionResult GetAdmin()
        {
            return Ok(new { message = "Hello from admin .NET API!", timestamp = DateTime.Now });
        }
        // simple test route to verify authorization is working
        [Authorize(Roles = "user")]
        [HttpGet("user")]
        public IActionResult GetUser()
        {
            return Ok(new { message = "Hello from User .NET API!", timestamp = DateTime.Now });
        }

    }




}