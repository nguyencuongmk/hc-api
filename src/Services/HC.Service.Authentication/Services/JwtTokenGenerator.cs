using HC.Foundation.Data.Entities;
using HC.Service.Authentication.Models;
using HC.Service.Authentication.Services.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HC.Service.Authentication.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public (string, DateTime) GenerateAccessToken(User user, IEnumerable<string> roles)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtOptions.Secret);
                var claimList = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Email,user.Email),
                    new(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                    new(JwtRegisteredClaimNames.Name,user.UserName)
                };

                claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                var expires = DateTime.UtcNow.AddHours(1);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = _jwtOptions.Audience,
                    Issuer = _jwtOptions.Issuer,
                    Subject = new ClaimsIdentity(claimList),
                    Expires = expires,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return (tokenHandler.WriteToken(token), expires);
            }
            catch
            {
                return (string.Empty, new DateTime());
            }
        }

        public (string, DateTime) GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var expires = DateTime.Now.AddHours(7);

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return (Convert.ToBase64String(randomNumber), expires);
            }
        }

        public ClaimsPrincipal ValidateToken(string accessToken)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.ReadToken(accessToken) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Encoding.UTF8.GetBytes(_jwtOptions.Secret);
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

                var principal = jwtTokenHandler.ValidateToken(accessToken, validationParameters, out SecurityToken validateToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}