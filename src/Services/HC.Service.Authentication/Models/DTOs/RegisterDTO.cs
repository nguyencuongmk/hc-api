namespace HC.Service.Authentication.Models.DTOs
{
    public class RegisterDTO
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }
}