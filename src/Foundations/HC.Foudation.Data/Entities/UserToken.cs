using HC.Foundation.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Foundation.Data.Entities
{
    public class UserToken : BaseEntity
    {
        public int UserId { get; set; }

        public string Type { get; set; }

        public string Token { get; set; }

        public DateTime ExpiredTime { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}