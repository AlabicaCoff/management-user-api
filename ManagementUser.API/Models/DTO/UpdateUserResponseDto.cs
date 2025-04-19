namespace ManagementUser.API.Models.DTO
{
    public class UpdateUserResponseDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }

        // Relationship
        public UserTitleDto? Role { get; set; }

        public string UserName { get; set; }

        // Relationship
        public List<ApplicationUserPermissionDto> Permissions { get; set; } = new List<ApplicationUserPermissionDto>();
    }
}
