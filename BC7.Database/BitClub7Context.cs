using BC7.Database.Extensions;
using BC7.Domain;
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

        public DbSet<UserAccountData> UserAccountsData { get; set; }
        public DbSet<UserMultiAccount> UserMultiAccounts { get; set; }
        public DbSet<MatrixPosition> MatrixPositions { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}
