using HC.Foundation.Data.Entities;
using System.Security.Claims;

namespace HC.Service.Authentication.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        (string, DateTime) GenerateAccessToken(User user, IEnumerable<string> roles);

        (string, DateTime) GenerateRefreshToken();

        ClaimsPrincipal ValidateToken(string accessToken);
    }
}
