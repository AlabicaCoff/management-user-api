using ManagementUser.API.Models.Domain;

namespace ManagementUser.API.Repositories.Interface
{
    public interface IUserTitleRepository
    {
        Task<IEnumerable<UserTitle>> GetAllAsync();
        Task<UserTitle> GetByIdAsync(Guid id);
    }
}
