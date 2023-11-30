using HC.Foundation.Data.Base;
using HC.Service.Email.Entities;
using HC.Service.Email.Repositories.IRepositories;

namespace HC.Service.Email.Repositories
{
    public class EmailLoggerRepository : BaseRepository<EmailLogger>, IEmailLoggerRepository
    {
        public EmailLoggerRepository(BaseDbContext context) : base(context)
        {
        }
    }
}
