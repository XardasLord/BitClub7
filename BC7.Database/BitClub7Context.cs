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
            modelBuilder.Entity<UserAccountData>().HasKey(x => x.Id);
            modelBuilder.Entity<UserAccountData>().Property(x => x.Id).ValueGeneratedOnAdd();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // TODO: Auto inserting dates?

            return await base.SaveChangesAsync(cancellationToken);
        }


        public DbSet<UserAccountData> UserAccountsData { get; set; }
    }
}
