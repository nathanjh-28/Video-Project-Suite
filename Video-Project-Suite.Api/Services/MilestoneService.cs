using System;
using Video_Project_Suite.Api.Models.Milestone;

namespace Video_Project_Suite.Api.Services;

public class MilestoneService : IMilestoneService
{
    public Task<bool> ChangeMilestonePositionAsync(int id, int newPosition)
    {
        throw new NotImplementedException();
    }

    public Task<Milestone> CreateMilestoneAsync(Milestone milestone)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteMilestoneAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Milestone>> GetAllMilestonesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Milestone> GetMilestoneByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Milestone> UpdateMilestoneAsync(int id, Milestone milestone)
    {
        throw new NotImplementedException();
    }
}
