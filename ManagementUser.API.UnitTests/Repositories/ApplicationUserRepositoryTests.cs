using ManagementUser.API.Data;
using ManagementUser.API.Repositories.Implementation;
using ManagementUser.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace ManagementUser.API.UnitTests.Repositories
{
    public class ApplicationUserRepositoryTests
    {
        private async Task<ApplicationDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var dbContext = new ApplicationDbContext(options);

            await dbContext.Database.EnsureCreatedAsync();

            // Clear seed data from ApplicationDbContext.OnModelCreating so tests control the exact dataset.
            dbContext.ApplicationUserPermissions.RemoveRange(dbContext.ApplicationUserPermissions);
            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async Task ApplicationUserRepository_CreateAsync_AddsNewApplicationUser()
        {
            // Arrange
            var dbContext = await GetInMemoryDbContext();
            var repository = new ApplicationUserPermissionRepository(dbContext);
            var newUser = new ApplicationUserPermission
            {
                Id = Guid.NewGuid(),
                UserId = "testuser",
                PermissionId = Guid.NewGuid()
            };

            // Act
            var result = await repository.CreateAsync(newUser);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ApplicationUserPermission>();
            result.Should().BeEquivalentTo(newUser, options => options.Excluding(u => u.User).Excluding(u => u.Permission));
        }

        [Fact]
        public async Task ApplicationUserRepository_DeleteRangeAsync_RemovesApplicationUserPermissions()
        {
            // Arrange
            var dbContext = await GetInMemoryDbContext();
            var repository = new ApplicationUserPermissionRepository(dbContext);
            var newUser = new ApplicationUserPermission
            {
                Id = Guid.NewGuid(),
                UserId = "testuser",
                PermissionId = Guid.NewGuid()
            };

            var createdUser = await repository.CreateAsync(newUser);

            // Act
            await repository.DeleteRangeAsync(new List<ApplicationUserPermission> { createdUser });

            // Assert
            var deletedUser = await dbContext.ApplicationUserPermissions.FindAsync(createdUser.Id);
            deletedUser.Should().BeNull();
        }
    }
}