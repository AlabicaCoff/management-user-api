using ManagementUser.API.Models.Domain;

namespace ManagementUser.API.Models.DTO
{
    public class UserTitleDto
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
