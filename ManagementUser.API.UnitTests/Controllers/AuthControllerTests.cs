using FakeItEasy;
using FluentAssertions;
using MockQueryable.FakeItEasy;
using ManagementUser.API.Controllers;
using ManagementUser.API.Models.Domain;
using ManagementUser.API.Models.DTO;
using ManagementUser.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ManagementUser.API.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthControllerTests()
        {
            _userManager = A.Fake<UserManager<ApplicationUser>>();
            _tokenRepository = A.Fake<ITokenRepository>();
        }

        [Fact]
        public async Task AuthController_Login_Returns_Ok()
        {
            // Arrange
            var loginRequest = A.Fake<LoginRequestDto>();

            var loginResponse = A.Fake<LoginResponseDto>();

            var identityUser = A.Fake<ApplicationUser>();
            identityUser.Id = "test-user-id";

            var userData = A.Fake<ApplicationUser>();
            userData.Id = "test-user-id";
            userData.ApplicationUserPermissions = new List<ApplicationUserPermission> { A.Fake<ApplicationUserPermission>() };
            userData.ApplicationUserPermissions.First().Permission = A.Fake<Permission>();
            userData.ApplicationUserPermissions.First().Permission.Name = "TestPermission";

            var users = new List<ApplicationUser> { userData };

            var roles = A.Fake<List<string>>();

            A.CallTo(() => _userManager.FindByEmailAsync(loginRequest.Email))
                .Returns(Task.FromResult<ApplicationUser?>(identityUser));
            A.CallTo(() => _userManager.CheckPasswordAsync(identityUser, loginRequest.Password))
                .Returns(Task.FromResult(true));
            A.CallTo(() => _userManager.Users)
                .Returns(users.AsQueryable().BuildMock());
            A.CallTo(() => _userManager.GetRolesAsync(identityUser))
                .Returns(Task.FromResult<IList<string>>(roles));
            A.CallTo(() => _tokenRepository.CreateJwtToken(A<ApplicationUser>.Ignored, A<List<string>>.Ignored))
                .Returns(loginResponse.Token);

            var controller = new AuthController(_userManager, _tokenRepository);

            // Act
            var result = await controller.Login(loginRequest);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeOfType<LoginResponseDto>();
        }
    }
}