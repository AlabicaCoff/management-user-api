using ManagementUser.API.Models.DTO;
using ManagementUser.API.Repositories.Implementation;
using ManagementUser.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagementUser.API.Controllers
{
    // https://localhost:7263/api/permissions
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionRepository permissionRepository;

        public PermissionsController(IPermissionRepository permissionRepository)
        {
            this.permissionRepository = permissionRepository;
        }

        // GET: {apiBaseUrl}/api/permissions?sort=true
        [HttpGet]
        [Authorize(Roles = "isWritable")]
        public async Task<IActionResult> GetAllPermissions([FromQuery] bool sort)
        {
            // Get All UserTitles from the DB
            var permissions = await permissionRepository.GetAllAsync(sort);

            // Convert Domain Model to DTO
            var response = new List<PermissionDto>();

            foreach (var permission in permissions)
            {
                response.Add(new PermissionDto
                {
                    PermissionId = permission.Id.ToString(),
                    PermissionName = permission.Name,
                    IsReadable = permission.IsReadable,
                    IsWritable = permission.IsWritable,
                    IsDeleatable = permission.IsDeleatable
                });
            }

            var apiResponse = new
            {
                status = new
                {
                    code = "Success",
                    description = "Permissions retrieved successfully"
                },
                data = response 
            };

            return Ok(apiResponse);
        }

    }
}
