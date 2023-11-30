using HC.Service.Email.Services.IServices;

namespace HC.Service.Email.Services
{
    public class EmailService : IEmailService
    {
        public Task<bool> SendEmailConfirmedAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
