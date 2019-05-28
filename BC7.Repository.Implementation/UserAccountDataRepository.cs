using System;
using System.Threading.Tasks;
using BC7.Database;
using BC7.Domain;
using Microsoft.EntityFrameworkCore;

namespace BC7.Repository.Implementation
{
    public class UserAccountDataRepository : IUserAccountDataRepository
    {
        private readonly IBitClub7Context _context;

        public UserAccountDataRepository(IBitClub7Context context)
        {
            _context = context;
        }

        public Task<UserAccountData> GetAsync(Guid id)
        {
            return _context.Set<UserAccountData>()
                .Include(x => x.UserMultiAccounts)
                .ThenInclude(x => x.MatrixPositions)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task<UserAccountData> GetAsync(string emailOrLogin)
        {
            return _context.Set<UserAccountData>()
                .Include(x => x.UserMultiAccounts)
                .ThenInclude(x => x.MatrixPositions)
                .SingleOrDefaultAsync(x => x.Email == emailOrLogin || x.Login == emailOrLogin);
        }

        public Task UpdateAsync(UserAccountData userAccountData)
        {
            _context.Set<UserAccountData>().Attach(userAccountData);
            return _context.SaveChangesAsync();
        }
    }
}
