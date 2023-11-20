namespace HC.Service.Authentication.Models.Requests
{
    public class RegisterRequest
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }
    }
}