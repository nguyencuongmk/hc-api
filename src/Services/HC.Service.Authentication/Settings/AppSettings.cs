namespace HC.Service.Authentication.Settings
{
    public class AppSettings
    {
        public JwtSettings JwtSettings { get; set; }
    }

    public class JwtSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Secret { get; set; }
    }
}