using HC.Foundation.Data.Base;
using HC.Foundation.Data.Entities;
using HC.Service.Authentication.Data;
using HC.Service.Authentication.Helpers;
using HC.Service.Authentication.Models;
using HC.Service.Authentication.Repositories.IRepositories;
using Microsoft.Extensions.Options;

namespace HC.Service.Authentication.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly JwtOptions _jwtOptions;

        public UserRepository(AuthenticationDbContext context, IOptions<JwtOptions> jwtOptions) : base(context)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<bool> AddToRoleAsync(User user, Role role)
        {
            if (role == null)
                return false;

            user.Roles.Add(role);
            return await UpdateAsync(user) != null;
        }

        public async Task<bool> AddToUserTokenAsync(User user, UserToken userToken)
        {
            if (user == null)
                return false;

            user.UserTokens.Add(userToken);
            return await UpdateAsync(user) != null;
        }

        public bool CheckPassword(User user, string requestPassword)
        {
            if (user == null || string.IsNullOrEmpty(requestPassword))
                return false;

            var password = PasswordHelper.DecodeFrom64(user.PasswordHash);

            if (string.IsNullOrEmpty(password) || !password.Equals(requestPassword, StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        public async Task<bool> CreateAsync(User user, string password)
        {
            if (user == null || string.IsNullOrEmpty(password))
                return false;

            var passwordHash = PasswordHelper.EncodePasswordToBase64(password);

            if (string.IsNullOrEmpty(passwordHash))
                return false;

            user.PasswordHash = passwordHash;
            user.CreatedBy = "system";

            return await AddAsync(user) != null;
        }

        public List<string> GetRoles(User user)
        {
            var result = new List<string>();

            if (user == null || user.Roles.Count == 0)
                return result;

            return user.Roles.Select(x => x.Name).ToList();
        }
    }
}