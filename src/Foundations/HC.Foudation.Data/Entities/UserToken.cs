using HC.Foundation.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;
using static HC.Foundation.Core.Constants.Constants;

namespace HC.Foundation.Data.Entities
{
    public class UserToken : IBaseInfo
    {
        public int UserId { get; set; }

        public string Type { get; set; }

        public string Token { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Status Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}