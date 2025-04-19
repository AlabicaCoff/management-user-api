using ManagementUser.API.Data;
using ManagementUser.API.Models.Domain;
using ManagementUser.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ManagementUser.API.Repositories.Implementation
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PermissionRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync(bool? sort = null)
        {
            var permissions = await dbContext.Permissions.ToListAsync();

            if (sort == true)
            {
                permissions = permissions
                    .OrderByDescending(p =>
                    {
                        if (p.IsReadable && p.IsWritable && p.IsDeleatable) return 1;
                        if (!p.IsReadable && p.IsWritable && p.IsDeleatable) return 2;
                        if (p.IsReadable && p.IsWritable && !p.IsDeleatable) return 3;
                        if (!p.IsReadable && !p.IsWritable && p.IsDeleatable) return 4;
                        if (!p.IsReadable && p.IsWritable && !p.IsDeleatable) return 5;
                        if (p.IsReadable && !p.IsWritable && !p.IsDeleatable) return 6;
                        return 10;
                    })
                    .ToList();
            }

            return permissions;
        }

        public async Task<Permission> GetByIdAsync(Guid id)
        {
            return await dbContext.Permissions.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
