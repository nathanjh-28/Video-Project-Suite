using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Video_Project_Suite.Api.Models.Milestone;
using Video_Project_Suite.Api.Services;

namespace Video_Project_Suite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MilestoneController(IMilestoneService milestoneService) : ControllerBase
    {

        // get all milestones

        [HttpGet]
        public async Task<IActionResult> GetAllMilestones()
        {
            var milestones = await milestoneService.GetAllMilestonesAsync();
            return Ok(milestones);
        }

        // get milestone by id

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMilestoneById(int id)
        {
            var milestone = await milestoneService.GetMilestoneByIdAsync(id);
            if (milestone == null)
            {
                return NotFound();
            }
            return Ok(milestone);
        }

        // create new milestone

        [HttpPost]
        public async Task<IActionResult> CreateMilestone(MilestoneDto milestoneDto)
        {
            if (milestoneDto == null)
            {
                return BadRequest("Milestone data is required.");
            }
            var createdMilestone = await milestoneService.CreateMilestoneAsync(milestoneDto);
            return Ok(createdMilestone);
        }

        // update milestone

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMilestone(int id, MilestoneDto milestone)
        {
            if (milestone == null)
            {
                return BadRequest("Milestone data is required.");
            }

            var updatedMilestone = await milestoneService.UpdateMilestoneAsync(id, milestone);
            if (updatedMilestone == null)
            {
                return BadRequest("Failed to update milestone position.");
            }
            return Ok(updatedMilestone);
        }

        // delete milestone

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMilestone(int id)
        {
            var deletedMilestone = await milestoneService.DeleteMilestoneAsync(id);
            if (deletedMilestone == null)
            {
                // invalid input
                return BadRequest("Invalid milestone ID.");
            }
            return Ok(deletedMilestone);
        }

        // change position on milestone

        [HttpPut("{id}/position")]
        public async Task<IActionResult> ChangeMilestonePosition(int id, int newPosition)
        {
            var success = await milestoneService.ChangeMilestonePositionAsync(id, newPosition);
            if (success == false)
            {
                return BadRequest("Invalid milestone ID or position.");
            }
            else
            {
                return Ok();
            }
        }

    }
}
