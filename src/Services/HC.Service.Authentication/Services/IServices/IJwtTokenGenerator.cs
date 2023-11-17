using HC.Foundation.Data.Entities;

namespace HC.Service.Authentication.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, IEnumerable<string> roles);
    }
}
