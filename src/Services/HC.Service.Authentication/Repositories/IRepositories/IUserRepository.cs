using HC.Foundation.Data.Base.IBase;
using HC.Service.Authentication.Entities;

namespace HC.Service.Authentication.Repositories.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> CreateAsync(User user, string password);

        Task<bool> AddToRoleAsync(User user, Role role);

        bool CheckPassword(User user, string password);

        List<string> GetRoles(User user);

        Task<bool> AddToUserTokenAsync(User user, UserToken userToken);
    }
}