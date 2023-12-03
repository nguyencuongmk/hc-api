using HC.Foundation.Data.Base;

namespace HC.Service.Email.Entities
{
    public class EmailLogger : BaseEntity
    {
        public string EmailReceiver { get; set; }

        public string Type { get; set; } // eg: Active Account

        public string Subject { get; set; } 

        public string Content { get; set; } 

        public DateTime? SentAt { get; set; }
    }
}
