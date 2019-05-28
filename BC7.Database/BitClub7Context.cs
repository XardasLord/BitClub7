using System.Threading;
using System.Threading.Tasks;
using BC7.Database.Extensions;
using BC7.Domain;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace BC7.Database
{
    public class BitClub7Context : DbContext, IBitClub7Context
    {
        public BitClub7Context(DbContextOptions<BitClub7Context> options)
            : base(options)
        {
            AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) =>
            {
                (context as BitClub7Context)?.AuditEntries.AddRange(audit.Entries);
            };
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
            modelBuilder.Configuration();
        }

        public Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var audit = new Audit();
            audit.PreSaveChanges(this);
            var rowsAffected = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            audit.PostSaveChanges();

            if (audit.Configuration.AutoSavePreAction != null)
            {
                audit.Configuration.AutoSavePreAction(this, audit);
                await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            return rowsAffected;
        }

        public DbSet<UserAccountData> UserAccountsData { get; set; }
        public DbSet<UserMultiAccount> UserMultiAccounts { get; set; }
        public DbSet<MatrixPosition> MatrixPositions { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<AuditEntry> AuditEntries { get; set; }
        public DbSet<AuditEntryProperty> AuditEntryProperties { get; set; }
    }
}
