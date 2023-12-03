using HC.Service.Authentication.Models.Requests;
using HC.Service.Authentication.Models.Responses;

namespace HC.Service.Authentication.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegisterRequest request);

        Task<string> AssignRole(string email, string roleName);

        Task<(LoginResponse, string)> Login(LoginRequest request);
    }
}
