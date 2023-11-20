using HC.Foundation.Data.Base;

namespace HC.Foundation.Data.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; } = false;

        public string PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public bool IsActive { get; set; } = false;

        public virtual List<UserRole> UserRoles { get; set; }

        public virtual List<UserToken> UserTokens { get; set; }
    }
}