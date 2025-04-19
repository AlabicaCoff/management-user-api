using ManagementUser.API.Models.DTO;
using ManagementUser.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagementUser.API.Controllers
{
    // https://localhost:7263/api/roles
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IUserTitleRepository userTitleRepository;

        public RolesController(IUserTitleRepository userTitleRepository)
        {
            this.userTitleRepository = userTitleRepository;
        }

        // GET: {apiBaseUrl}/api/roles
        [HttpGet]
        [Authorize(Roles = "isWritable")]
        public async Task<IActionResult> GetAllUserTitles()
        {
            // Get All UserTitles from the DB
            var userTitles = await userTitleRepository.GetAllAsync();

            // Convert Domain Model to DTO
            var response = new List<UserTitleDto>();

            foreach (var title in userTitles)
            {
                response.Add(new UserTitleDto
                {
                    RoleId = title.Id,
                    RoleName = title.Name,
                });
            }

            var apiResponse = new
            {
                status = new
                {
                    code = "Success",
                    description = "Roles retrieved successfully"
                },
                data = response
            };

            return Ok(apiResponse);
        }

    }
}
