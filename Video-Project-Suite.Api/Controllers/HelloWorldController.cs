using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Video_Project_Suite.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello, World!");
        }
    }
}
