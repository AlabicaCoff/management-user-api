using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementUser.API.Models.Domain
{
    public class ApplicationUser: IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }

        [PersonalData]
        public DateTime CreatedDate { get; set; }


        // Relationships
        public Guid UserTitleId { get; set; }

        [ForeignKey("UserTitleId")]
        public UserTitle UserTitle { get; set; }

        public ICollection<ApplicationUserPermission> ApplicationUserPermissions { get; set; }
    }
}
