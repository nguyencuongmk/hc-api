using HC.Foundation.Data.Base;
using HC.Foundation.Data.Entities;
using HC.Service.Authentication.Data;
using HC.Service.Authentication.Repositories.IRepositories;

namespace HC.Service.Authentication.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AuthenticationDbContext context) : base(context)
        {
        }
    }
}