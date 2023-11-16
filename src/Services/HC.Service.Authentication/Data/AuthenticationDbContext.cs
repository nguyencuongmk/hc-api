using HC.Foundation.Data.Base;
using HC.Foundation.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HC.Service.Authentication.Data
{
    public class AuthenticationDbContext : DbContext
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserToken>().HasKey(uk => new { uk.UserId, uk.Type });
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            BeforeSaveChanges();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BeforeSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        public void BeforeSaveChanges()
        {
            var entities = ChangeTracker.Entries();
            var now = DateTime.Now;
            foreach (var entity in entities)
            {
                if (entity.Entity is IBaseEntity baseEntity)
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            baseEntity.CreatedOn = now;
                            baseEntity.UpdatedOn = now;
                            break;

                        case EntityState.Modified:
                            baseEntity.UpdatedOn = now;
                            break;
                    }
            }
        }
    }
}