using HC.Foundation.Data.Base;

namespace HC.Foundation.Data.Entities
{
    public class User : UserBaseEntity
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
    }
}