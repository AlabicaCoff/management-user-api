using ManagementUser.API.Models.Domain;

namespace ManagementUser.API.Repositories.Interface
{
    public interface IApplicationUserPermissionRepository
    {
        Task<ApplicationUserPermission> CreateAsync(ApplicationUserPermission applicationUserPermission);
        Task DeleteRangeAsync(IEnumerable<ApplicationUserPermission> applicationUserPermissions);
    }
}
