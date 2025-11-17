using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Video_Project_Suite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MilestoneController : ControllerBase
    {

        // get all milestones

        [HttpGet]
        public async Task<IActionResult> GetAllMilestones()
        {
            return Ok("Get All Milestones");
        }

        // get milestone by id

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMilestoneById(int id)
        {
            return Ok($"Get Milestone By Id: {id}");
        }

        // create new milestone

        [HttpPost]
        public async Task<IActionResult> CreateMilestone()
        {
            return Ok("Create Milestone");
        }

        // update milestone

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMilestone(int id)
        {
            return Ok($"Update Milestone: {id}");
        }

        // delete milestone

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMilestone(int id)
        {
            return Ok($"Delete Milestone: {id}");
        }

        // change position on milestone

        [HttpPut("{id}/position")]
        public async Task<IActionResult> ChangeMilestonePosition(int id, int newPosition)
        {
            return Ok($"Change Milestone Position: {id} to {newPosition}");
        }

    }
}
