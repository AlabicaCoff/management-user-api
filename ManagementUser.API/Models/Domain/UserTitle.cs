namespace ManagementUser.API.Models.Domain
{
    public class UserTitle
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Relationship
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
