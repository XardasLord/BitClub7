using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BC7.Database
{
    public interface IBitClub7Context : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        DatabaseFacade Database { get; }

        DbSet<UserAccountData> UserAccountsData { get; set; }
        DbSet<UserMultiAccount> UserMultiAccounts { get; set; }
        DbSet<MatrixPosition> MatrixPositions { get; set; }
    }
}
