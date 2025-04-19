using ManagementUser.API.Data;
using ManagementUser.API.Models.Domain;
using ManagementUser.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ManagementUser.API.Repositories.Implementation
{
    public class UserTitleRepository : IUserTitleRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserTitleRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<UserTitle>> GetAllAsync()
        {
            return await dbContext.UserTitles.ToListAsync();
        }

        public async Task<UserTitle> GetByIdAsync(Guid id)
        {
            return await dbContext.UserTitles.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
