using System;
using Microsoft.EntityFrameworkCore;
using Video_Project_Suite.Api.Data;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Models.Project;

namespace Video_Project_Suite.Api.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;

    public ProjectService(AppDbContext context)
    {
        _context = context;
    }

    #region CreateProjectAsync

    public async Task<ProjectDto?> CreateProjectAsync(ProjectDto newProjectDto)
    {
        if (newProjectDto.ShortName == null || newProjectDto.ShortName.Length < 2)
        {
            return null;
        }

        var newProject = new Project
        {
            ShortName = newProjectDto.ShortName,
            Title = newProjectDto.Title,
            Focus = newProjectDto.Focus,
            Scope = newProjectDto.Scope,
            PricePerUnit = newProjectDto.PricePerUnit,
            QtyOfUnits = newProjectDto.QtyOfUnits,
            ExpenseBudget = newProjectDto.ExpenseBudget,
            ExpenseSummary = newProjectDto.ExpenseSummary,
            Comments = newProjectDto.Comments,
            StartDate = newProjectDto.StartDate,
            EndDate = newProjectDto.EndDate,
            MilestoneId = newProjectDto.MilestoneId,
            Type = newProjectDto.Type
        };

        var entry = _context.Project.Add(newProject);
        await _context.SaveChangesAsync();

        if (entry.Entity.Id <= 0)
        {
            return null; // Return null if the project was not created successfully
        }

        // Map the created project to ProjectDto
        var createdProject = new ProjectDto
        {
            Id = entry.Entity.Id,
            ShortName = entry.Entity.ShortName,
            Title = entry.Entity.Title,
            Focus = entry.Entity.Focus,
            Scope = entry.Entity.Scope,
            PricePerUnit = entry.Entity.PricePerUnit,
            QtyOfUnits = entry.Entity.QtyOfUnits,
            ExpenseBudget = entry.Entity.ExpenseBudget,
            ExpenseSummary = entry.Entity.ExpenseSummary,
            Comments = entry.Entity.Comments,
            StartDate = entry.Entity.StartDate,
            EndDate = entry.Entity.EndDate,
            MilestoneId = entry.Entity.MilestoneId,
            Type = entry.Entity.Type
        };

        return createdProject; // Return the created project DTO
    }

    #endregion

    #region DeleteProjectAsync
    public async Task<ProjectDto?> DeleteProjectAsync(int projectId)
    {
        var project = await _context.Project.FindAsync(projectId);
        if (project == null)
        {
            return null; // Return null if the project does not exist
        }

        var entity = _context.Project.Remove(project);
        await _context.SaveChangesAsync();
        var deletedProjectDto = new ProjectDto
        {
            Id = entity.Entity.Id,
            Title = entity.Entity.Title,
            ShortName = entity.Entity.ShortName,
            Focus = entity.Entity.Focus,
            Scope = entity.Entity.Scope,
            PricePerUnit = entity.Entity.PricePerUnit,
            QtyOfUnits = entity.Entity.QtyOfUnits,
            ExpenseBudget = entity.Entity.ExpenseBudget,
            ExpenseSummary = entity.Entity.ExpenseSummary,
            Comments = entity.Entity.Comments,
            StartDate = entity.Entity.StartDate,
            EndDate = entity.Entity.EndDate,
            MilestoneId = entity.Entity.MilestoneId,
            Type = entity.Entity.Type
        };


        return deletedProjectDto;


        // return deletedProjectDto;
    }

    #endregion

    #region GetAllProjectsAsync
    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _context.Project.ToListAsync();
        if (projects == null || !projects.Any())
        {
            return Enumerable.Empty<ProjectDto>(); // Return an empty list if no projects exist
        }

        return projects.Select(p => new ProjectDto
        {
            // need all the properties in ProjectDto
            Id = p.Id,
            Title = p.Title,
            ShortName = p.ShortName,
            Focus = p.Focus,
            Scope = p.Scope,
            PricePerUnit = p.PricePerUnit,
            QtyOfUnits = p.QtyOfUnits,
            ExpenseBudget = p.ExpenseBudget,
            ExpenseSummary = p.ExpenseSummary,
            Comments = p.Comments,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            MilestoneId = p.MilestoneId,
            Type = p.Type

        });

    }

    #endregion

    #region GetProjectByIdAsync

    public async Task<ProjectDto?> GetProjectByIdAsync(int projectId)
    {
        var project = await _context.Project.FindAsync(projectId);
        if (project == null)
        {
            return null;
        }
        var retrieved_project = new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            ShortName = project.ShortName,
            Focus = project.Focus,
            Scope = project.Scope,
            PricePerUnit = project.PricePerUnit,
            QtyOfUnits = project.QtyOfUnits,
            ExpenseBudget = project.ExpenseBudget,
            ExpenseSummary = project.ExpenseSummary,
            Comments = project.Comments,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            MilestoneId = project.MilestoneId,
            Type = project.Type
        };

        return retrieved_project;
    }

    #endregion

    #region UpdateProjectAsync
    public async Task<ProjectDto?> UpdateProjectAsync(int projectId, ProjectDto projectDto)
    {

        var oldProject = await _context.Project.FindAsync(projectId);
        if (oldProject == null)
        {
            return null;
        }
        else
        {
            oldProject.Title = projectDto.Title;
            oldProject.ShortName = projectDto.ShortName;
            oldProject.Focus = projectDto.Focus;
            oldProject.Scope = projectDto.Scope;
            oldProject.PricePerUnit = projectDto.PricePerUnit;
            oldProject.QtyOfUnits = projectDto.QtyOfUnits;
            oldProject.ExpenseBudget = projectDto.ExpenseBudget;
            oldProject.ExpenseSummary = projectDto.ExpenseSummary;
            oldProject.Comments = projectDto.Comments;
            oldProject.StartDate = projectDto.StartDate;
            oldProject.EndDate = projectDto.EndDate;
            oldProject.MilestoneId = projectDto.MilestoneId;
            oldProject.Type = projectDto.Type;

            _context.Project.Update(oldProject);
            _context.SaveChanges();

            var newProjectUpdated = await _context.Project.FindAsync(projectId);
            if (newProjectUpdated == null)
            {
                return null; // Return null if the project was not updated successfully
            }
            // Map the updated project to ProjectDto
            var updatedProjectDto = new ProjectDto
            {
                Id = newProjectUpdated.Id,
                Title = newProjectUpdated.Title,
                ShortName = newProjectUpdated.ShortName,
                Focus = newProjectUpdated.Focus,
                Scope = newProjectUpdated.Scope,
                PricePerUnit = newProjectUpdated.PricePerUnit,
                QtyOfUnits = newProjectUpdated.QtyOfUnits,
                ExpenseBudget = newProjectUpdated.ExpenseBudget,
                ExpenseSummary = newProjectUpdated.ExpenseSummary,
                Comments = newProjectUpdated.Comments,
                StartDate = newProjectUpdated.StartDate,
                EndDate = newProjectUpdated.EndDate,
                MilestoneId = newProjectUpdated.MilestoneId,
                Type = newProjectUpdated.Type
            };

            return updatedProjectDto;
        }
    }

    public async Task<bool> UpdateProjectMilestoneAsync(int projectId, int milestoneId)
    {
        var project = await _context.Project.FindAsync(projectId);

        if (project == null)
        {
            return false;
        }

        project.MilestoneId = milestoneId;
        _context.Project.Update(project);
        await _context.SaveChangesAsync();
        return true;
    }
    #endregion
}


