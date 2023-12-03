using Newtonsoft.Json;

namespace HC.Service.Authentication.Models.Requests
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
