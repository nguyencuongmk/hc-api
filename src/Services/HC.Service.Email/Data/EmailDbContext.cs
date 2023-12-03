using HC.Foundation.Data.Base;
using HC.Service.Email.Entities;
using Microsoft.EntityFrameworkCore;

namespace HC.Service.Email.Data
{
    public class EmailDbContext : BaseDbContext
    {
        public EmailDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<EmailLogger> EmailLoggers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
