using ManagementUser.API.Data;
using ManagementUser.API.Repositories.Implementation;
using ManagementUser.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace ManagementUser.API.UnitTests.Repositories
{
    public class UserTitleRepositoryTests
    {
        private async Task<ApplicationDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var dbContext = new ApplicationDbContext(options);

            await dbContext.Database.EnsureCreatedAsync();

            // Clear seed data from ApplicationDbContext.OnModelCreating so tests control the exact dataset.
            dbContext.UserTitles.RemoveRange(dbContext.UserTitles);
            await dbContext.SaveChangesAsync();

            dbContext.UserTitles.AddRange(
                new UserTitle
                {
                    Id = Guid.NewGuid(),
                    Name = "User Title 1"
                },
                new UserTitle
                {
                    Id = Guid.NewGuid(),
                    Name = "User Title 2"
                },
                new UserTitle
                {
                    Id = Guid.NewGuid(),
                    Name = "User Title 3"
                }
            );
            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async Task UserTitlesRepository_GetAllAsync_ReturnsAllUserTitles()
        {
            // Arrange
            var dbContext = await GetInMemoryDbContext();
            var repository = new UserTitleRepository(dbContext);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task UserTitlesRepository_GetByIdAsync_ReturnsCorrectUserTitle()
        {
            // Arrange
            var dbContext = await GetInMemoryDbContext();
            var repository = new UserTitleRepository(dbContext);
            var existingUserTitle = await dbContext.UserTitles.FirstAsync();

            // Act
            var result = await repository.GetByIdAsync(existingUserTitle.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(existingUserTitle.Id);
            result.Name.Should().Be(existingUserTitle.Name);
        }
    }
}