using System;
using Microsoft.EntityFrameworkCore;

using Video_Project_Suite.Api.Data;
using Video_Project_Suite.Api.Services;



using Video_Project_Suite.Api.Models.UserProject;

namespace Video_Project_Suite.Api.Tests.Services
{
    public class UserProjectServiceTests : IDisposable
    {
        private readonly AppDbContext _context;

        private readonly UserProjectService _userProjectService;

        public UserProjectServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _userProjectService = new UserProjectService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // Helper Method to convert UserProject to UserProjectDto

        // Helper Method to convert UserProjectDto to UserProject

        // Helper Method to seed some Users and Projects


        // TESTS

        #region GetAllUserProjectsAsync
        [Fact]
        public async Task GetAllUserProjectsAsync_ReturnsAllUserProjects()
        {
            // Arrange

            // Act

            // Assert

            Assert.Equal(0, 0);
        }
        #endregion

    }
}