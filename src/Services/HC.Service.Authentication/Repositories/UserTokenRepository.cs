using HC.Foundation.Data.Base;
using HC.Foundation.Data.Entities;
using HC.Service.Authentication.Repositories.IRepositories;

namespace HC.Service.Authentication.Repositories
{
    public class UserTokenRepository : BaseRepository<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(BaseDbContext context) : base(context)
        {
        }
    }
}
