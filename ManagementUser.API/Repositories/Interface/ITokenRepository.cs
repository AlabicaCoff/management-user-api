using ManagementUser.API.Models.Domain;

namespace ManagementUser.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(ApplicationUser user, List<string> roles);
    }
}
