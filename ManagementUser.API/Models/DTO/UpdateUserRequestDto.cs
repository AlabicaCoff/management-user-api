﻿namespace ManagementUser.API.Models.DTO
{
    public class UpdateUserRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }

        // Relationship
        public string RoleId { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        // Relationship
        public UpdateUserPermissionDto[] Permissions { get; set; }
    }
}
