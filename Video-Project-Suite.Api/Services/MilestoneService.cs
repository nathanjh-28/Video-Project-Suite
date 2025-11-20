using System;
using Microsoft.EntityFrameworkCore;
using Video_Project_Suite.Api.Data;
using Video_Project_Suite.Api.Models.Milestone;

namespace Video_Project_Suite.Api.Services;

public class MilestoneService : IMilestoneService
{

    private readonly AppDbContext _context;

    public MilestoneService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> ChangeMilestonePositionAsync(int id, int newPosition)
    {
        // get milestone by id
        var milestone = await _context.Milestones.FindAsync(id);

        if (milestone == null)
        {
            return false;
        }

        Console.WriteLine($"Milestone Id: {milestone.Id}, milestone Name: {milestone.Name}, Position: {milestone.Position}");

        // get and sort list of milestones by position
        var milestones = await _context.Milestones.OrderBy(m => m.Position).ToListAsync();

        foreach (var m in milestones)
        {
            Console.WriteLine($"Milestone Id: {m.Id}, milestone Name: {m.Name}, Position: {m.Position}");
        }
        // check that new position is valid
        if (newPosition < 0 || newPosition >= milestones.Count)
        {
            return false;
        }

        // remove milestone from current position
        milestones.Remove(milestone);

        milestone.Position = newPosition;

        Console.WriteLine("removed target miestone");
        foreach (var m in milestones)
        {
            Console.WriteLine($"Milestone Id: {m.Id}, milestone Name: {m.Name}, Position: {m.Position}");
        }
        // insert in the list at new position
        milestones.Insert(newPosition - 1, milestone);
        Console.WriteLine("inserted target milestone at new position");
        foreach (var m in milestones)
        {
            Console.WriteLine($"Milestone Id: {m.Id}, milestone Name: {m.Name}, Position: {m.Position}");
        }

        // update positions of all milestones in the list
        foreach (var (m, index) in milestones.Select((value, i) => (value, i)))
        {
            if (m.Id == id) // skip the inserted milestone
            {
                continue;
            }
            else
            {
                m.Position = index + 1;
            }
        }

        Console.WriteLine("updated positions of all milestones");
        foreach (var m in milestones)
        {
            Console.WriteLine($"Milestone Id: {m.Id}, milestone Name: {m.Name}, Position: {m.Position}");
        }

        // save new list of milestones to database
        _context.Milestones.UpdateRange(milestones);

        await _context.SaveChangesAsync();

        Console.WriteLine("saved changes to database");
        return true;

    }

    public async Task<Milestone?> CreateMilestoneAsync(MilestoneDto milestone)
    {
        // is milestone position valid

        var lastMilestone = await _context.Milestones.OrderByDescending(m => m.Position).FirstOrDefaultAsync();

        if (milestone.Position < 0 || (lastMilestone != null && milestone.Position > lastMilestone.Position + 1))
        {
            return null;
        }

        var newMilestone = new Milestone
        {
            Name = milestone.Name,
            Position = milestone.Position
        };

        if (lastMilestone != null && milestone.Position <= lastMilestone.Position)
        {
            // need to increment positions of milestones at or after the new milestone's position
            var milestonesToUpdate = await _context.Milestones
                .Where(m => m.Position >= milestone.Position)
                .ToListAsync();

            foreach (var m in milestonesToUpdate)
            {
                m.Position += 1;
            }

            _context.Milestones.UpdateRange(milestonesToUpdate);
        }

        var entry = _context.Milestones.Add(newMilestone);
        await _context.SaveChangesAsync();

        if (entry.Entity.Id <= 0)
        {
            return null;
        }

        var createdMilestone = new Milestone
        {
            Id = entry.Entity.Id,
            Name = entry.Entity.Name,
            Position = entry.Entity.Position
        };

        return createdMilestone;

    }

    public async Task<Milestone?> DeleteMilestoneAsync(int id)
    {
        if (id <= 0)
        {
            return null;
        }
        var milestone = await _context.Milestones.FindAsync(id);
        if (milestone == null)
        {
            return null;
        }

        var deletedPosition = milestone.Position;

        // Remove the milestone
        _context.Milestones.Remove(milestone);

        // Update positions of all milestones after the deleted one
        var milestonesToUpdate = await _context.Milestones
            .Where(m => m.Position > deletedPosition)
            .ToListAsync();

        foreach (var m in milestonesToUpdate)
        {
            m.Position -= 1;
        }

        if (milestonesToUpdate.Any())
        {
            _context.Milestones.UpdateRange(milestonesToUpdate);
        }

        await _context.SaveChangesAsync();
        return milestone;
    }

    public async Task<IEnumerable<Milestone>> GetAllMilestonesAsync()
    {
        var milestones = await _context.Milestones.ToListAsync();
        if (milestones == null)
        {
            return Enumerable.Empty<Milestone>();
        }
        return milestones;
    }

    public async Task<Milestone?> GetMilestoneByIdAsync(int id)
    {
        var milestone = await _context.Milestones.FindAsync(id);
        if (milestone == null)
        {
            return null;
        }
        return milestone;
    }

    public async Task<Milestone?> UpdateMilestoneAsync(int id, MilestoneDto milestone)
    {
        var existingMilestone = await _context.Milestones.FindAsync(id);
        if (existingMilestone == null)
        {
            return null!;
        }
        existingMilestone.Name = milestone.Name;

        var success = await ChangeMilestonePositionAsync(id, milestone.Position);
        if (success == false)
        {
            return null;
        }

        await _context.SaveChangesAsync();
        return existingMilestone;
    }
}
