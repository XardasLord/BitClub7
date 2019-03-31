using System.Threading;
using System.Threading.Tasks;
using BC7.Database.Extensions;
using BC7.Entity;
using Microsoft.EntityFrameworkCore;

namespace BC7.Database
{
    public class BitClub7Context : DbContext, IBitClub7Context
    {
        public BitClub7Context(DbContextOptions<BitClub7Context> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
            modelBuilder.Configuration();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.SaveChangesAsync(cancellationToken);
        }


        public DbSet<UserAccountData> UserAccountsData { get; set; }
        public DbSet<UserMultiAccount> UserMultiAccounts { get; set; }
        public DbSet<MatrixPosition> MatrixPositions { get; set; }
    }
}
