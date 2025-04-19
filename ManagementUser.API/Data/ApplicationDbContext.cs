using ManagementUser.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ManagementUser.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var isReadableRoleId = "9ab55ee9-52a9-45c9-ae26-1013c376b383";
            var isWritableRoleId = "2f3acff7-944b-4c05-83dc-a4175d93a018";
            var isDeletableRoleId = "c6b9517f-4a39-4154-abb1-9811d825059b";

            // Create Roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = isReadableRoleId,
                    Name = "isReadable",
                    NormalizedName = "isReadable".ToUpper(),
                    ConcurrencyStamp = isReadableRoleId
                },
                new IdentityRole()
                {
                    Id = isWritableRoleId,
                    Name = "isWritable",
                    NormalizedName = "isWritable".ToUpper(),
                    ConcurrencyStamp = isWritableRoleId
                },
                new IdentityRole()
                {
                    Id = isDeletableRoleId,
                    Name = "isDeletable",
                    NormalizedName = "isDeletable".ToUpper(),
                    ConcurrencyStamp = isDeletableRoleId
                }
            };

            // Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            // Create a Super Admin User
            var superAdminId = "47d9b5c2-9aff-484d-87b9-e8ebe16a5868";
            var superAdmin = new ApplicationUser()
            {
                Id = superAdminId,
                FirstName = "Super",
                LastName = "Admin",
                Email = "SuperAdmin@ManagementUser.com",
                NormalizedEmail = "SuperAdmin@ManagementUser.com".ToUpper(),
                UserName = "SuperAdmin@ManagementUser.com",
                NormalizedUserName = "SuperAdmin@ManagementUser.com".ToUpper(),
                CreatedDate = DateTime.Now,
            };

            // Add Super Admin User Password
            superAdmin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(superAdmin, "SuperAdmin@123");

            // Add UserTitle to Super Admin User
            var superAdminTitleId = Guid.Parse("2b28190b-de83-4263-8ad4-a69407f72113");
            var superAdminTitle = new UserTitle()
            {
                Id = superAdminTitleId,
                Name = "Super Admin"
            };

            builder.Entity<UserTitle>().HasData(superAdminTitle);

            superAdmin.UserTitleId = superAdminTitleId;

            // Add Permission to DB
            var superAdminPermissionId = Guid.Parse("1bbc8dd6-43ac-48bd-8c1a-72fe753cdb61");
            var superAdminPermission = new Permission()
            {
                Id = superAdminPermissionId,
                Name = "Super Admin",
                IsReadable = true,
                IsWritable = true,
                IsDeleatable = true
            };

            builder.Entity<Permission>().HasData(superAdminPermission);

            // Add Super Admin User to Database
            builder.Entity<ApplicationUser>().HasData(superAdmin);

            // Add Permission to Super Admin User
            var superAdminPermissionJoin = new ApplicationUserPermission
            {
                Id = Guid.Parse("d6931b44-e2fd-4f7b-9503-ff2f77976737"),
                UserId = superAdminId,
                PermissionId = superAdminPermissionId
            };

            builder.Entity<ApplicationUserPermission>().HasData(superAdminPermissionJoin);

            // Give Roles To Super Admin User
            var superAdminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = superAdminId,
                    RoleId = isReadableRoleId
                },
                new()
                {
                    UserId = superAdminId,
                    RoleId = isWritableRoleId
                },
                new()
                {
                    UserId = superAdminId,
                    RoleId = isDeletableRoleId
                },
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);

            // Add more UserTitles
            var userTitles = new List<UserTitle>()
            {
                new UserTitle()
                {
                    Id = Guid.Parse("fe041e1f-6a8c-431f-8f71-ae0ec3cdcf17"),
                    Name = "CEO"
                },
                new UserTitle()
                {
                    Id = Guid.Parse("5d0b040d-36bc-48e8-960e-0e5564fcd3ee"),
                    Name = "CTO"
                },
                new UserTitle()
                {
                    Id = Guid.Parse("d41a4de8-69f0-46c2-bfea-c1037f09d5c3"),
                    Name = "HR Manager"
                },
                new UserTitle()
                {
                    Id = Guid.Parse("d11f7b70-e56d-47f3-abec-065233b16739"),
                    Name = "Sales"
                },
                new UserTitle()
                {
                    Id = Guid.Parse("18cb7a6e-e953-45ab-9ccd-a04743dfa9fa"),
                    Name = "Accountant"
                },
            };

            // Add more Permissions
            var permissions = new List<Permission>()
            {
                new Permission()
                {
                    Id = Guid.Parse("3d408d29-dd7d-4c17-b670-ba1ae8f6b30e"),
                    Name = "Admin",
                    IsReadable = true,
                    IsWritable = true,
                    IsDeleatable = false
                },
                new Permission()
                {
                    Id = Guid.Parse("4181a1f0-5e9b-47b2-bed8-0fc232089ebe"),
                    Name = "HR Admin",
                    IsReadable = true,
                    IsWritable = true,
                    IsDeleatable = true
                },
                new Permission()
                {
                    Id = Guid.Parse("fc07f013-1939-4618-9617-0098aba1e71a"),
                    Name = "HR",
                    IsReadable = true,
                    IsWritable = false,
                    IsDeleatable = false
                },
                new Permission()
                {
                    Id = Guid.Parse("94f06510-d4ac-4594-9462-79553d7a686d"),
                    Name = "Employee",
                    IsReadable = true,
                    IsWritable = false,
                    IsDeleatable = false
                },
                new Permission()
                {
                    Id = Guid.Parse("8ec3db5a-0eed-4e47-a9fb-dab325d35690"),
                    Name = "Intern",
                    IsReadable = false,
                    IsWritable = false,
                    IsDeleatable = false
                }
            };


            // Add more UserTitles and Permissions to DB
            for (int i = 0; i < permissions.Count; i++)
            {
                builder.Entity<UserTitle>().HasData(userTitles[i]);
                builder.Entity<Permission>().HasData(permissions[i]);
            }

        }


        public DbSet<UserTitle> UserTitles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ApplicationUserPermission> ApplicationUserPermissions { get; set; }
    }
}
