using HC.Foundation.Data.Base;
using HC.Foundation.Data.Entities;
using HC.Service.Authentication.Data;
using HC.Service.Authentication.Helpers;
using HC.Service.Authentication.Models;
using HC.Service.Authentication.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace HC.Service.Authentication.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly JwtOptions _jwtOptions;
        private readonly AuthenticationDbContext _context;

        public UserRepository(AuthenticationDbContext context, IOptions<JwtOptions> jwtOptions) : base(context)
        {
            _jwtOptions = jwtOptions.Value;
            _context = context;
        }

        public async Task<bool> AddToRoleAsync(User user, Role role)
        {
            user.Roles.Add(role);

            var entries = _context.ChangeTracker.Entries();
            return await UpdateAsync(user) != null;
        }

        public async Task<bool> CreateAsync(User user, string password)
        {
            var passwordHash = PasswordHelper.EncodePasswordToBase64(password);

            if (string.IsNullOrEmpty(passwordHash))
                return false;

            user.PasswordHash = passwordHash;

            var newuser = await AddAsync(user);
            var entries = _context.ChangeTracker.Entries();

            return newuser != null;
        }

        public async Task<bool> Verify(string accessToken)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.ReadToken(accessToken) as JwtSecurityToken;

                if (jwtToken == null)
                    return false;

                var symmetricKey = Convert.FromBase64String(_jwtOptions.Secret);
                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidIssuer = _jwtOptions.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(0),
                    ValidateAudience = true,
                    ValidAudience = _jwtOptions.Audience,
                };
                var principal = await jwtTokenHandler.ValidateTokenAsync(accessToken, validationParameters);

                return principal.IsValid;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}