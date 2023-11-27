using HC.Foundation.Data.Entities;
using HC.Service.Authentication.Models;
using HC.Service.Authentication.Services.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public string GenerateToken(User user, IEnumerable<string> roles)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
                var claimList = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Email,user.Email),
                    new(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                    new(JwtRegisteredClaimNames.Name,user.UserName)
                };

                claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = _jwtOptions.Audience,
                    Issuer = _jwtOptions.Issuer,
                    Subject = new ClaimsIdentity(claimList),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}