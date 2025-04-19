namespace ManagementUser.API.Models.DTO
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string permissionName { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
