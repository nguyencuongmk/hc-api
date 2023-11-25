using HC.Foundation.Data.Base.IBase;
using HC.Foundation.Data.Entities;

namespace HC.Service.Authentication.Repositories.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> Verify(string accessToken);

        Task<bool> CreateAsync(User user, string password);

        Task<bool> AddToRoleAsync(User user, Role role);
    }
}