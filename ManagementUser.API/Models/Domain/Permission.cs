namespace ManagementUser.API.Models.Domain
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsReadable { get; set; }
        public bool IsWritable { get; set; }
        public bool IsDeleatable { get; set; }

        // Relationship
        public ICollection<ApplicationUserPermission> ApplicationUserPermissions { get; set; }
    }
}
