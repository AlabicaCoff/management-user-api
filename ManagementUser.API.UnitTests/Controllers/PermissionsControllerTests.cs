using FakeItEasy;
using FluentAssertions;
using ManagementUser.API.Controllers;
using ManagementUser.API.Models.Domain;
using ManagementUser.API.Models.DTO;
using ManagementUser.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ManagementUser.API.UnitTests.Controllers
{
    public class PermissionsControllerTests
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionsControllerTests()
        {
            _permissionRepository = A.Fake<IPermissionRepository>();
        }

        [Fact]
        public async Task PermissionsController_GetAllPermissions_Returns_Ok()
        {
            // Arrange
            var permissions = new List<Permission>();
            A.CallTo(() => _permissionRepository.GetAllAsync(false))
                .Returns(Task.FromResult((IEnumerable<Permission>)permissions));
            var controller = new PermissionsController(_permissionRepository);

            // Act
            var result = await controller.GetAllPermissions();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeOfType<ApiResponse<List<PermissionDto>>>();
        }
    }
}
