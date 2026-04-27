using FakeItEasy;
using FluentAssertions;
using ManagementUser.API.Controllers;
using ManagementUser.API.Models.Domain;
using ManagementUser.API.Models.DTO;
using ManagementUser.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ManagementUser.API.UnitTests.Controllers
{
    public class RolesControllerTests
    {
        private readonly IUserTitleRepository _userTitleRepository;

        public RolesControllerTests()
        {
            _userTitleRepository = A.Fake<IUserTitleRepository>();
        }

        [Fact]
        public async Task RolesController_GetAllUserTitles_Returns_Ok()
        {
            // Arrange
            var userTitles = new List<UserTitle>();
            A.CallTo(() => _userTitleRepository.GetAllAsync())
                .Returns(Task.FromResult((IEnumerable<UserTitle>)userTitles));
            var controller = new RolesController(_userTitleRepository);

            // Act
            var result = await controller.GetAllUserTitles();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeOfType<ApiResponse<List<UserTitleDto>>>();
        }
    }
}