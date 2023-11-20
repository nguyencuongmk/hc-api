using HC.Service.Authentication.Models.Requests;

namespace HC.Service.Authentication.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegisterRequest request);
    }
}
