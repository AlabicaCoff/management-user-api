using ManagementUser.API.Data;
using ManagementUser.API.Repositories.Implementation;
using ManagementUser.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace ManagementUser.API.UnitTests.Repositories
{
    public class PermissionRepositoryTests
    {
        private async Task<ApplicationDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var dbContext = new ApplicationDbContext(options);

            await dbContext.Database.EnsureCreatedAsync();

            // Clear seed data from ApplicationDbContext.OnModelCreating so tests control the exact dataset.
            dbContext.Permissions.RemoveRange(dbContext.Permissions);
            await dbContext.SaveChangesAsync();

            dbContext.Permissions.AddRange(
                new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = "Permission 1",
                    IsReadable = true,
                    IsWritable = true,
                    IsDeleatable = true
                },
                new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = "Permission 2",
                    IsReadable = false,
                    IsWritable = true,
                    IsDeleatable = true
                },
                new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = "Permission 3",
                    IsReadable = true,
                    IsWritable = true,
                    IsDeleatable = false
                }
            );
            await dbContext.SaveChangesAsync();

            return dbContext;
        }

        [Fact]
        public async Task PermissionsRepository_GetAllAsync_ReturnsAllPermissions()
        {
            // Arrange
            var dbContext = await GetInMemoryDbContext();
            var repository = new PermissionRepository(dbContext);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task PermissionsRepository_GetByIdAsync_ReturnsCorrectPermission()
        {
            // Arrange
            var dbContext = await GetInMemoryDbContext();
            var repository = new PermissionRepository(dbContext);
            var existingPermission = dbContext.Permissions.First();

            // Act
            var result = await repository.GetByIdAsync(existingPermission.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(existingPermission.Id);
        }
    }
}