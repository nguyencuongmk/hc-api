namespace HC.Service.Email.Services.IServices
{
    public interface IEmailService
    {
        Task<bool> SendEmailConfirmedAsync(string email);
    }
}
