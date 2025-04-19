using ManagementUser.API.Data;
using ManagementUser.API.Models.Domain;
using ManagementUser.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ManagementUser.API.Repositories.Implementation
{
    public class ApplicationUserPermissionRepository : IApplicationUserPermissionRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ApplicationUserPermissionRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ApplicationUserPermission> CreateAsync(ApplicationUserPermission applicationUserPermission)
        {
            await dbContext.ApplicationUserPermissions.AddAsync(applicationUserPermission);
            await dbContext.SaveChangesAsync();

            return applicationUserPermission;
        }

        public async Task DeleteRangeAsync(IEnumerable<ApplicationUserPermission> applicationUserPermissions)
        {
            dbContext.ApplicationUserPermissions.RemoveRange(applicationUserPermissions);
            await dbContext.SaveChangesAsync();
        }
    }
}
