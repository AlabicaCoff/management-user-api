using ManagementUser.API.Models.Domain;

namespace ManagementUser.API.Repositories.Interface
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllAsync(bool? sort = null);
        Task<Permission> GetByIdAsync(Guid id);
    }
}
