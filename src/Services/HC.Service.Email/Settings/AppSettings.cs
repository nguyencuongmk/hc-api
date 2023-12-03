namespace HC.Service.Email.Settings
{
    public class AppSettings
    {
        public MailSettings MailSettings { get; set; }
    }

    public class MailSettings
    {
        public bool EnableSsl { get; set; }

        public string MailServerUsername { get; set; }

        public string MailServerPassword { get; set; }

        public string MailServer { get; set; }

        public string MailServerPort { get; set; }
    }
}
