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
    public class UsersControllerTests
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserTitleRepository _userTitleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IApplicationUserPermissionRepository _applicationUserPermissionRepository;

        public UsersControllerTests()
        {
            _userManager = A.Fake<UserManager<ApplicationUser>>();
            _userTitleRepository = A.Fake<IUserTitleRepository>();
            _permissionRepository = A.Fake<IPermissionRepository>();
            _applicationUserPermissionRepository = A.Fake<IApplicationUserPermissionRepository>();

        }

        [Fact]
        public async Task UsersController_GetAllUsers_Returns_Ok()
        {
            // Arrange
            var request = A.Fake<GetAllUsersRequestDto>();

            var userData = A.Fake<ApplicationUser>();
            userData.Id = "test-user-id";
            userData.ApplicationUserPermissions = new List<ApplicationUserPermission> { A.Fake<ApplicationUserPermission>() };
            userData.ApplicationUserPermissions.First().Permission = A.Fake<Permission>();
            userData.UserTitle = A.Fake<UserTitle>();

            var users = new List<ApplicationUser> { userData };

            var orderBy = request.orderBy;
            var orderDirection = request.orderDirection;
            var pageNumber = request.pageNumber;
            var pageSize = request.pageSize;
            var search = request.search;
            var totalCount = users.Count();

            A.CallTo(() => _userManager.Users)
                .Returns(users.AsQueryable().BuildMock());
            var controller = new UsersController(_userManager, _userTitleRepository, _permissionRepository, _applicationUserPermissionRepository);

            // Act
            var result = await controller.GetAllUsers(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeOfType<ApiResponsePagination<List<UserDto>>>();
        }
        
        [Fact]
        public async Task UsersController_GetUserById_Returns_Ok()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userData = A.Fake<ApplicationUser>();
            userData.Id = id.ToString();
            userData.ApplicationUserPermissions = new List<ApplicationUserPermission> { A.Fake<ApplicationUserPermission>() };
            userData.ApplicationUserPermissions.First().Permission = A.Fake<Permission>();
            userData.UserTitle = A.Fake<UserTitle>();

            var users = new List<ApplicationUser> { userData };

            A.CallTo(() => _userManager.Users)
                .Returns(users.AsQueryable().BuildMock());
            var controller = new UsersController(_userManager, _userTitleRepository, _permissionRepository, _applicationUserPermissionRepository);

            // Act
            var result = await controller.GetUserById(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeOfType<ApiResponse<UserDto>>();
        }

        [Fact]
        public async Task UsersController_CreateUser_Returns_Ok()
        {         
            // Arrange
            var request = A.Fake<CreateUserRequestDto>();
            request.RoleId = Guid.NewGuid().ToString();
            request.Permissions = new CreateUserPermissionDto[] { A.Fake<CreateUserPermissionDto>() };
            request.Permissions.First().PermissionId = Guid.NewGuid().ToString();

            var user = A.Fake<ApplicationUser>();
            user.Id = request.Id;
            
            A.CallTo(() => _userManager.CreateAsync(A<ApplicationUser>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(IdentityResult.Success));
            A.CallTo(() => _permissionRepository.GetByIdAsync(A<Guid>.Ignored))
                .Returns(Task.FromResult<Permission>(A.Fake<Permission>()));
            A.CallTo(() => _userManager.AddToRoleAsync(A<ApplicationUser>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(IdentityResult.Success));

            var controller = new UsersController(_userManager, _userTitleRepository, _permissionRepository, _applicationUserPermissionRepository);

            // Act\
            var result = await controller.CreateUser(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>();

            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeOfType<ApiResponse<UserDto>>();
        }

        [Fact]
        public async Task UsersController_UpdateUser_Returns_Ok()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = A.Fake<UpdateUserRequestDto>();
            request.RoleId = Guid.NewGuid().ToString();
            request.Password = "NewPassword123!";
            request.Permissions = new UpdateUserPermissionDto[] { A.Fake<UpdateUserPermissionDto>() };
            request.Permissions.First().PermissionId = Guid.NewGuid().ToString();

            var existingUser = A.Fake<ApplicationUser>();
            existingUser.Id = id.ToString();
            existingUser.ApplicationUserPermissions = new List<ApplicationUserPermission> { A.Fake<ApplicationUserPermission>() };
            existingUser.ApplicationUserPermissions.First().Permission = A.Fake<Permission>();
            existingUser.UserTitle = A.Fake<UserTitle>();

            A.CallTo(() => _userManager.Users)
                .Returns(new List<ApplicationUser> { existingUser }.AsQueryable().BuildMock());
            A.CallTo(() => _userManager.RemoveFromRolesAsync(A<ApplicationUser>.Ignored, A<List<string>>.Ignored))
                .Returns(Task.FromResult(IdentityResult.Success));
            A.CallTo(() => _applicationUserPermissionRepository.DeleteRangeAsync(existingUser.ApplicationUserPermissions))
                .Returns(Task.CompletedTask);
            A.CallTo(() => _userTitleRepository.GetByIdAsync(A<Guid>.Ignored))
                .Returns(Task.FromResult<UserTitle>(A.Fake<UserTitle>()));
            A.CallTo(() => _userManager.UpdateAsync(A<ApplicationUser>.Ignored))
                .Returns(Task.FromResult(IdentityResult.Success));
            A.CallTo(() => _permissionRepository.GetByIdAsync(A<Guid>.Ignored))
                .Returns(Task.FromResult<Permission>(A.Fake<Permission>()));
            A.CallTo(() => _applicationUserPermissionRepository.CreateAsync(A<ApplicationUserPermission>.Ignored))
                .Returns(Task.FromResult(A.Fake<ApplicationUserPermission>()));
            A.CallTo(() => _userManager.AddToRoleAsync(A<ApplicationUser>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(IdentityResult.Success));
        
            var controller = new UsersController(_userManager, _userTitleRepository, _permissionRepository, _applicationUserPermissionRepository);

            // Act
            var result = await controller.UpdateUser(id, request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>();

            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeOfType<ApiResponse<UserDto>>();
        }

        [Fact]
        public async Task UsersController_DeleteUser_Returns_Ok()
        {
            // Arrange
            var id = Guid.NewGuid();

            var existingUser = A.Fake<ApplicationUser>();
            existingUser.Id = id.ToString();

            var userPermissions = new List<ApplicationUserPermission> { A.Fake<ApplicationUserPermission>() };
            existingUser.ApplicationUserPermissions = userPermissions;
            existingUser.ApplicationUserPermissions.First().Permission = A.Fake<Permission>();
            existingUser.UserTitle = A.Fake<UserTitle>();

            var existingPermissions = userPermissions.Select(up => up.Permission).ToList();

            A.CallTo(() => _userManager.Users)
                .Returns(new List<ApplicationUser> { existingUser }.AsQueryable().BuildMock());
            A.CallTo(() => _userManager.DeleteAsync(A<ApplicationUser>.Ignored))
                .Returns(Task.FromResult(IdentityResult.Success));

            var controller = new UsersController(_userManager, _userTitleRepository, _permissionRepository, _applicationUserPermissionRepository);

            // Act
            var result = await controller.DeleteUser(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>();

            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeOfType<ApiResponse<string>>();
        }

        [Fact]
        public async Task UsersController_GetUserId_Returns_Ok()
        {
            // Arrange
            
            var controller = new UsersController(_userManager, _userTitleRepository, _permissionRepository, _applicationUserPermissionRepository);

            // Act
            var result = await controller.GetUserId();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeOfType<NewUserIdResponse>();
        }
    }
}