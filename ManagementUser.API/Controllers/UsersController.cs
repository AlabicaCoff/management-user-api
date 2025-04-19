using Azure;
using Azure.Core;
using ManagementUser.API.Models.Domain;
using ManagementUser.API.Models.DTO;
using ManagementUser.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ManagementUser.API.Controllers
{
    // https://localhost:7263/api/users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserTitleRepository userTitleRepository;
        private readonly IPermissionRepository permissionRepository;
        private readonly IApplicationUserPermissionRepository applicationUserPermissionRepository;

        public UsersController(UserManager<ApplicationUser> userManager, IUserTitleRepository userTitleRepository, IPermissionRepository permissionRepository, IApplicationUserPermissionRepository applicationUserPermissionRepository)
        {
            this.userManager = userManager;
            this.userTitleRepository = userTitleRepository;
            this.permissionRepository = permissionRepository;
            this.applicationUserPermissionRepository = applicationUserPermissionRepository;
        }

        // POST: {apiBaseUrl}/api/users/DataTable
        [HttpPost]
        [Route("DataTable")]
        [Authorize(Roles = "isReadable")]
        public async Task<IActionResult> GetAllUsers([FromBody] GetAllUsersRequestDto request)
        {
            // Declare variables
            var orderBy = request.orderBy;
            var orderDirection = request.orderDirection;
            var pageNumber = request.pageNumber;
            var pageSize = request.pageSize;
            var search = request.search;

            var totalCount = await userManager.Users.CountAsync();

            // Get all users from database
            var users = userManager.Users.Include(x => x.ApplicationUserPermissions).ThenInclude(x => x.Permission).Include(x => x.UserTitle).AsQueryable();

            // Searching or Filtering
            if (string.IsNullOrWhiteSpace(search) == false)
            {
                // Filter Users by Firstname, Lastname, Email, Role and Permission
                users = users.Where(x => x.FirstName.Contains(search) || 
                    x.LastName.Contains(search) || x.Email.Contains(search) 
                    || x.UserTitle.Name.Contains(search)
                    || x.ApplicationUserPermissions.Select(x => x.Permission.Name).First().Contains(search));
            }

            // Sorting by Firstname, Lastname, Email, CreatedDate
            if (string.IsNullOrWhiteSpace(orderBy) == false)
            {
                if (string.Equals(orderBy, "Firstname", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(orderDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    users = isAsc ? users.OrderBy(x => x.FirstName) : users.OrderByDescending(x => x.FirstName);
                }

                if (string.Equals(orderBy, "Lastname", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(orderDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    users = isAsc ? users.OrderBy(x => x.LastName) : users.OrderByDescending(x => x.LastName);
                }

                if (string.Equals(orderBy, "Email", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(orderDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    users = isAsc ? users.OrderBy(x => x.Email) : users.OrderByDescending(x => x.Email);
                }

                if (string.Equals(orderBy, "CreatedDate", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(orderDirection, "asc", StringComparison.OrdinalIgnoreCase)
                        ? true : false;

                    users = isAsc ? users.OrderBy(x => x.CreatedDate) : users.OrderByDescending(x => x.CreatedDate);
                }
            }

            // Pagination
            // Pagenumber 1 pagesize 5 - skip 0, take 5  [1,2,3,4,5]
            // Pagenumber 2 pagesize 5 - skip 5, take 5, [6,7,8,9,10]
            // Pagenumber 3 pagesize 5 - skip 10, take 5 [11,12,13,14,15]

            var skipResults = (pageNumber - 1) * pageSize;
            users = users.Skip(skipResults ?? 0).Take(pageSize ?? 100);

            // Convert Domain model to DTO
            var response = new List<UserDto>();
            foreach (var user in users)
            {
                response.Add(new UserDto()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Role = new UserTitleDto
                    {
                        RoleId = user.UserTitle.Id,
                        RoleName = user.UserTitle.Name
                    },
                    UserName = user.UserName,
                    Permissions = user.ApplicationUserPermissions.Select(x => new ApplicationUserPermissionDto
                    {
                        PermissionId = x.PermissionId,
                        PermissionName = x.Permission.Name
                    }).ToList(),
                    CreatedDate = user.CreatedDate
                });
            }

            var apiResponse = new
            {
                dataSource = response,
                page = pageNumber,
                pageSize = pageSize,
                totalCount = totalCount
            };

            return Ok(apiResponse);
        }

        // GET: {apiBaseUrl}/api/users/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "isWritable")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            // Get the User from Database
            var user = await userManager.Users.Include(x => x.ApplicationUserPermissions).ThenInclude(x => x.Permission).Include(x => x.UserTitle).FirstOrDefaultAsync(x => x.Id == id.ToString());

            if (user is null)
            {
                return NotFound();
            }

            // Convert Domain Model to DTO
            var response = new UserDto()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Role = new UserTitleDto
                {
                    RoleId = user.UserTitle.Id,
                    RoleName = user.UserTitle.Name
                },
                UserName = user.UserName,
                Permissions = user.ApplicationUserPermissions.Select(x => new ApplicationUserPermissionDto
                {
                    PermissionId = x.PermissionId,
                    PermissionName = x.Permission.Name
                }).ToList(),
                CreatedDate = user.CreatedDate
            };
            
            var apiResponse = new
            {
                status = new
                {
                    code = "Success",
                    description = "User retrieved successfully"
                },
                data = response
            };

            return Ok(apiResponse);
        }

        // POST: {apiBaseUrl}/api/users
        [HttpPost]
        [Authorize(Roles = "isWritable")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request)
        {
            var roleId = Guid.Parse(request.RoleId);

            // Convert DTO to Domain Model
            var user = new ApplicationUser
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.Phone,
                UserTitleId = roleId,
                UserName = request.UserName,
                ApplicationUserPermissions = new List<ApplicationUserPermission>(),
                CreatedDate =  DateTime.Now
            };

            // Add UserTitle to User
            user.UserTitleId = roleId;
            user.UserTitle = await userTitleRepository.GetByIdAsync(roleId);

            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (identityResult.Succeeded)
            {
                var requestPermissions = new List<string>();

                // Add UserPermissions to UserPermissions Database
                foreach (var permission in request.Permissions)
                {
                    var existingPermission = await permissionRepository.GetByIdAsync(Guid.Parse(permission.PermissionId));

                    if (existingPermission is not null)
                    {
                        var userPermissions = new ApplicationUserPermission
                        {
                            UserId = request.Id,
                            PermissionId = existingPermission.Id,
                        };
                        userPermissions = await applicationUserPermissionRepository.CreateAsync(userPermissions);

                        if (existingPermission.IsReadable)
                        {
                            requestPermissions.Add("isReadable");
                        }

                        if (existingPermission.IsWritable)
                        {
                            requestPermissions.Add("isWritable");
                        }

                        if (existingPermission.IsDeleatable)
                        {
                            requestPermissions.Add("isDeletable");
                        }
                    }
                }

                // Add Permissions to user
                identityResult = await userManager.AddToRolesAsync(user, requestPermissions);

                if (identityResult.Succeeded)
                {
                    // Convert Domain Model back to DTO
                    var response = new CreateUserResponseDto
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                        Role = new UserTitleDto
                        {
                            RoleId = user.UserTitle.Id,
                            RoleName = user.UserTitle.Name
                        },
                        UserName = user.UserName,
                        Permissions = user.ApplicationUserPermissions.Select(x => new ApplicationUserPermissionDto
                        {
                            PermissionId = x.PermissionId,
                            PermissionName = x.Permission.Name
                        }).ToList(),
                    };

                    var apiResponse = new
                    {
                        status = new
                        {
                            code = "Success",
                            description = "User created successfully"
                        },
                        data = new[] { response }
                    };

                    return Ok(apiResponse);
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }

        // PUT: {apiBaseUrl}/api/users/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "isWritable")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, UpdateUserRequestDto request)
        {
            // Get the User from Database
            var existingUser = await userManager.Users.Include(x => x.ApplicationUserPermissions).ThenInclude(x => x.Permission).Include(x => x.UserTitle).FirstOrDefaultAsync(x => x.Id == id.ToString());

            if (existingUser is null)
            {
                return NotFound();
            }

            var removePermissions = new List<string>();

            var userPermissions = existingUser.ApplicationUserPermissions;
            var existingPermissions = userPermissions.Select(x => x.Permission).ToList();

            foreach (var permission in existingPermissions)
            {
                if (permission.IsReadable)
                {
                    removePermissions.Add("isReadable");
                }

                if (permission.IsWritable)
                {
                    removePermissions.Add("isWritable");
                }

                if (permission.IsDeleatable)
                {
                    removePermissions.Add("isDeletable");
                }
            }

            // Remove User Permissions
            var identityResult = await userManager.RemoveFromRolesAsync(existingUser, removePermissions);

            // Remove UserPermission from ApplicationUserPermission Database
            await applicationUserPermissionRepository.DeleteRangeAsync(userPermissions);

            if (identityResult.Succeeded)
            {
                var roleId = Guid.Parse(request.RoleId);

                // Convert Request DTO to Domain Model
                existingUser.FirstName = request.FirstName;
                existingUser.LastName = request.LastName;
                existingUser.Email = request.Email;
                existingUser.PhoneNumber = request.Phone;
                existingUser.UserTitleId = roleId;

                var updatedRole = userTitleRepository.GetByIdAsync(roleId).Result;
                existingUser.UserTitle = updatedRole;

                existingUser.UserName = request.UserName;
                existingUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(existingUser, request.Password);

                existingUser.ApplicationUserPermissions = new List<ApplicationUserPermission>();
                existingUser.CreatedDate = DateTime.Now;

                identityResult = await userManager.UpdateAsync(existingUser);

                if (identityResult.Succeeded)
                {
                    var requestPermissions = new List<string>();

                    // Add UserPermissions to UserPermissions Database
                    foreach (var permission in request.Permissions)
                    {
                        var existingRequestPermission = await permissionRepository.GetByIdAsync(Guid.Parse(permission.PermissionId));

                        if (existingRequestPermission is not null)
                        {
                            var updatedUserPermission = new ApplicationUserPermission
                            {
                                UserId = id.ToString(),
                                PermissionId = existingRequestPermission.Id,
                            };

                            updatedUserPermission = await applicationUserPermissionRepository.CreateAsync(updatedUserPermission);
                            existingUser.ApplicationUserPermissions.Add(updatedUserPermission);

                            if (existingRequestPermission.IsReadable)
                            {
                                requestPermissions.Add("isReadable");
                            }

                            if (existingRequestPermission.IsWritable)
                            {
                                requestPermissions.Add("isWritable");
                            }

                            if (existingRequestPermission.IsDeleatable)
                            {
                                requestPermissions.Add("isDeletable");
                            }
                        }
                    }

                    // Add Permissions to user
                    identityResult = await userManager.AddToRolesAsync(existingUser, requestPermissions);

                    if (identityResult.Succeeded)
                    {
                        // Convert Domain Model back to DTO
                        var response = new UpdateUserResponseDto
                        {
                            UserId = existingUser.Id,
                            FirstName = existingUser.FirstName,
                            LastName = existingUser.LastName,
                            Email = existingUser.Email,
                            Phone = existingUser.PhoneNumber,
                            Role = new UserTitleDto
                            {
                                RoleId = existingUser.UserTitle.Id,
                                RoleName = existingUser.UserTitle.Name
                            },
                            UserName = existingUser.UserName,
                            Permissions = existingUser.ApplicationUserPermissions.Select(x => new ApplicationUserPermissionDto
                            {
                                PermissionId = x.PermissionId,
                                PermissionName = x.Permission.Name
                            }).ToList(),
                        };

                        var apiResponse = new
                        {
                            status = new
                            {
                                code = "Success",
                                description = "User updated successfully"
                            },
                            data = response
                        };

                        return Ok(apiResponse);
                    }
                    else
                    {
                        if (identityResult.Errors.Any())
                        {
                            foreach (var error in identityResult.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }


            return ValidationProblem(ModelState);
        }

        // DELETE: {apiBaseUrl}/api/users/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "isDeletable")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            // Get the User from Database
            var existingUser = await userManager.Users.Include(x => x.ApplicationUserPermissions).ThenInclude(x => x.Permission).Include(x => x.UserTitle).FirstOrDefaultAsync(x => x.Id == id.ToString());

            if (existingUser is null)
            {
                return NotFound();
            }

            var removePermissions = new List<string>();

            var userPermissions = existingUser.ApplicationUserPermissions;
            var existingPermissions = userPermissions.Select(x => x.Permission).ToList();

            foreach (var permission in existingPermissions)
            {
                if (permission.IsReadable)
                {
                    removePermissions.Add("isReadable");
                }

                if (permission.IsWritable)
                {
                    removePermissions.Add("isWritable");
                }

                if (permission.IsDeleatable)
                {
                    removePermissions.Add("isDeletable");
                }
            }

            // Remove User Permissions
            var identityResult = await userManager.RemoveFromRolesAsync(existingUser, removePermissions);

            // Remove UserPermission from ApplicationUserPermission Database
            await applicationUserPermissionRepository.DeleteRangeAsync(userPermissions);

            if (identityResult.Succeeded)
            {
                identityResult = await userManager.DeleteAsync(existingUser);

                if (identityResult.Succeeded)
                {
                    var apiResponse = new
                    {
                        status = new
                        {
                            code = "Success",
                            description = "User deleted successfully"
                        },
                        data = new 
                        { 
                            result = true,
                            message = "User ID: " + id + " deleted successfully"
                        }
                    };

                    return Ok(apiResponse);
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }

        // GET: {apiBaseUrl}/api/users/get-id
        [HttpGet]
        [Route("get-id")]
        [Authorize(Roles = "isWritable")]
        public async Task<IActionResult> GetUserId()
        {
            var newUserId = Guid.NewGuid().ToString();

            var apiResponse = new
            {
                newUserId = newUserId,
            };

            return Ok(apiResponse);
        }
    }
}
