using HC.Foundation.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Service.Authentication.Entities
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}