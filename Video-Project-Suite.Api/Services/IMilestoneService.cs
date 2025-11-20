using System;
using Video_Project_Suite.Api.Models.Milestone;

namespace Video_Project_Suite.Api.Services;

public interface IMilestoneService
{

    // get all milestones

    Task<IEnumerable<Milestone>> GetAllMilestonesAsync();

    // get milestone by id

    Task<Milestone?> GetMilestoneByIdAsync(int id);

    // create new milestone

    Task<Milestone?> CreateMilestoneAsync(MilestoneDto milestone);

    // update milestone

    Task<Milestone?> UpdateMilestoneAsync(int id, MilestoneDto milestone);

    // delete milestone

    Task<Milestone?> DeleteMilestoneAsync(int id);

    // change position on milestone

    Task<bool> ChangeMilestonePositionAsync(int id, int newPosition);
}
