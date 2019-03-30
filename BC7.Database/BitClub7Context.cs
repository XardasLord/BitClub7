using System.Threading;
using System.Threading.Tasks;
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
            modelBuilder.Entity<UserAccountData>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Login).IsUnique();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Login).IsRequired();
                entity.Property(e => e.Salt).IsRequired();
                entity.Property(e => e.Hash).IsRequired();
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.BtcWalletAddress).IsRequired();
                entity.Property(e => e.Street).IsRequired();
                entity.Property(e => e.City).IsRequired();
                entity.Property(e => e.ZipCode).IsRequired();
                entity.Property(e => e.Country).IsRequired();
                entity.Property(e => e.Role).IsRequired();
            });

            modelBuilder.Entity<UserMultiAccount>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MatrixPosition>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // TODO: Auto inserting dates?

            return await base.SaveChangesAsync(cancellationToken);
        }


        public DbSet<UserAccountData> UserAccountsData { get; set; }
        public DbSet<UserMultiAccount> UserMultiAccounts { get; set; }
        public DbSet<MatrixPosition> MatrixPositions { get; set; }
    }
}
