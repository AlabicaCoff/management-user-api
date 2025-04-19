namespace ManagementUser.API.Models.DTO
{
    public class PermissionDto
    {
        public string PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool IsReadable { get; set; }
        public bool IsWritable { get; set; }
        public bool IsDeleatable { get; set; }
    }
}
