using HC.Service.Authentication.Models;
using HC.Service.Authentication.Repositories;
using HC.Service.Authentication.Repositories.IRepositories;
using Microsoft.Extensions.Options;

namespace HC.Service.Authentication.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthenticationDbContext _context;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IUserTokenRepository _userTokenRepository;
        private IOptions<JwtOptions> _jwtOptions;

        public UnitOfWork(AuthenticationDbContext context, IOptions<JwtOptions> jwtOptions)
        {
            _context = context;
            _jwtOptions = jwtOptions;
        }

        public AuthenticationDbContext Context => _context;

        public IUserRepository UserRepository
        {
            get
            {
                _userRepository ??= new UserRepository(_context, _jwtOptions);
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

        public async Task<bool> SaveChangesAsync()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
    }

    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }

        IRoleRepository RoleRepository { get; }

        IUserTokenRepository UserTokenRepository { get; }

        AuthenticationDbContext Context { get; }

        Task<bool> SaveChangesAsync();
    }
}
