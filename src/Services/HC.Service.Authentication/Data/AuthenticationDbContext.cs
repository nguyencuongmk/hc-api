using HC.Foundation.Data.Base;
using HC.Foundation.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HC.Service.Authentication.Data
{
    public class AuthenticationDbContext : BaseDbContext
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        //public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
            //modelBuilder.Entity<UserRole>().Ignore(p => p.Id);
            modelBuilder.Entity<User>().HasIndex(x => x.UserName).IsUnique();
            Initializer.SeedData(modelBuilder);
        }
    }
}