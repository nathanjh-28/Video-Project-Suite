using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Video_Project_Suite.Api.Data;
using Video_Project_Suite.Api.Models;
using Video_Project_Suite.Api.Models.Dto;
using Video_Project_Suite.Api.Services;
using Xunit;


namespace Video_Project_Suite.Api.Tests.Services
{
    public class ProjectServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        // private readonly IConfiguration _configuration;
        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            // Setup in-memory database for testing
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Setup configuration mock
            // _configuration = CreateConfiguration();


            _projectService = new ProjectService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        // [Fact]
        // public async Task CreateProject_ShouldAddProjectToDatabase()
        // {
        //     // tbd
        // }

    }
}

