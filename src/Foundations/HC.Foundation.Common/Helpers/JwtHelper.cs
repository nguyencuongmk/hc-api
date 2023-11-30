using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HC.Foundation.Common.Helpers
{
    public class JwtHelper
    {
        public static (string, DateTime) GenerateAccessToken(string email, int userId, string username, string issuer, string audience, string secretKey, IEnumerable<string> roles)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(secretKey);
                var claimList = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Email, email),
                    new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new(JwtRegisteredClaimNames.Name, username)
                };

                claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                var expires = DateTime.UtcNow.AddHours(1);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = audience,
                    Issuer = issuer,
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

        public static (string, DateTime) GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var expires = DateTime.Now.AddHours(7);

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return (Convert.ToBase64String(randomNumber), expires);
            }
        }

        public static ClaimsPrincipal ValidateToken(string accessToken, string issuer, string audience, string secretKey)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.ReadToken(accessToken) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Encoding.UTF8.GetBytes(secretKey);
                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(0),
                    ValidateAudience = true,
                    ValidAudience = audience,
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