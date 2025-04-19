namespace ManagementUser.API.Models.DTO
{
    public class UpdateUserPermissionDto
    {
        public string PermissionId { get; set; }
        public bool IsReadable { get; set; }
        public bool IsWritable { get; set; }
        public bool IsDeleatable { get; set; }
    }
}
