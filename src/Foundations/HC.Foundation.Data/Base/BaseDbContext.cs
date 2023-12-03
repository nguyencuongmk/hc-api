using HC.Foundation.Data.Base.IBase;
using Microsoft.EntityFrameworkCore;
using static HC.Foundation.Common.Constants.Constants;

namespace HC.Foundation.Data.Base
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            BeforeSaveChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BeforeSaveChanges();
            return await base.SaveChangesAsync(cancellationToken);
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
                            baseEntity.Status = Status.Created;
                            baseEntity.CreatedOn = now;
                            baseEntity.UpdatedOn = now;
                            break;

                        case EntityState.Modified:
                            baseEntity.Status = Status.Modified;
                            baseEntity.UpdatedOn = now;
                            break;
                    }
            }
        }
    }
}