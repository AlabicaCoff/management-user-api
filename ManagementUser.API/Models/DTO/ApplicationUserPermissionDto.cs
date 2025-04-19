using ManagementUser.API.Models.Domain;

namespace ManagementUser.API.Models.DTO
{
    public class ApplicationUserPermissionDto
    {
        public Guid PermissionId { get; set; }
        public string PermissionName { get; set; }
    }
}
