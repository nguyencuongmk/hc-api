using HC.Service.Email.Repositories;
using HC.Service.Email.Repositories.IRepositories;

namespace HC.Service.Email.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmailDbContext _context;
        private IEmailLoggerRepository _emailRepository;

        public UnitOfWork(EmailDbContext context)
        {
            _context = context;
        }

        public IEmailLoggerRepository EmailLoggerRepository
        {
            get 
            {
                _emailRepository ??= new EmailLoggerRepository(_context);
                return _emailRepository; 
            }
        }

        public EmailDbContext Context => throw new NotImplementedException();

        public void Dispose() => _context.Dispose();
    }

    public interface IUnitOfWork : IDisposable
    {
        IEmailLoggerRepository EmailLoggerRepository { get; }

        EmailDbContext Context { get; }
    }
}
