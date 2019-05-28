using System;
using System.Threading.Tasks;
using BC7.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Z.EntityFramework.Plus;

namespace BC7.Database
{
    public interface IBitClub7Context : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
        DatabaseFacade Database { get; }

        DbSet<UserAccountData> UserAccountsData { get; set; }
        DbSet<UserMultiAccount> UserMultiAccounts { get; set; }
        DbSet<MatrixPosition> MatrixPositions { get; set; }
        DbSet<PaymentHistory> PaymentHistories { get; set; }
        DbSet<Article> Articles { get; set; }
        DbSet<AuditEntry> AuditEntries { get; set; }
        DbSet<AuditEntryProperty> AuditEntryProperties { get; set; }
    }
}
