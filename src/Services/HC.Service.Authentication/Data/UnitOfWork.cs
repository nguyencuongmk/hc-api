using HC.Service.Authentication.Repositories;
using HC.Service.Authentication.Repositories.IRepositories;
using HC.Service.Authentication.Settings;
using Microsoft.Extensions.Options;

namespace HC.Service.Authentication.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthenticationDbContext _context;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IUserTokenRepository _userTokenRepository;
        private IOptions<AppSettings> _appSettings;

        public UnitOfWork(AuthenticationDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        public AuthenticationDbContext Context => _context;

        public IUserRepository UserRepository
        {
            get
            {
                _userRepository ??= new UserRepository(_context, _appSettings);
                return _userRepository;
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                _roleRepository ??= new RoleRepository(_context);
                return _roleRepository;
            }
        }

        public IUserTokenRepository UserTokenRepository
        {
            get
            {
                _userTokenRepository ??= new UserTokenRepository(_context);
                return _userTokenRepository;
            }
        }

        public void Dispose() => _context.Dispose();
    }

    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }

        IRoleRepository RoleRepository { get; }

        IUserTokenRepository UserTokenRepository { get; }

        AuthenticationDbContext Context { get; }
    }
}
