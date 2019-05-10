using System;
using System.Threading.Tasks;
using BC7.Database;
using BC7.Domain;
using Microsoft.EntityFrameworkCore;

namespace BC7.Repository.Implementation
{
    public class UserMultiAccountRepository : IUserMultiAccountRepository
    {
        private readonly IBitClub7Context _context;

        public UserMultiAccountRepository(IBitClub7Context context)
        {
            _context = context;
        }

        public Task<UserMultiAccount> GetAsync(Guid id)
        {
            return _context.Set<UserMultiAccount>()
                .Include(x => x.UserAccountData)
                .Include(x => x.MatrixPositions)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        // TODO: Some BrowseAsync with Func/Expression for these two functions below maybe?
        public Task<UserMultiAccount> GetByReflinkAsync(string reflink)
        {
            return _context.Set<UserMultiAccount>().SingleOrDefaultAsync(x => x.RefLink == reflink);
        }

        public Task<UserMultiAccount> GetByAccountNameAsync(string accountName)
        {
            return _context.Set<UserMultiAccount>().SingleOrDefaultAsync(x => x.MultiAccountName == accountName);
        }

        public Task CreateAsync(UserMultiAccount userMultiAccount)
        {
            _context.Set<UserMultiAccount>().Add(userMultiAccount);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(UserMultiAccount userMultiAccount)
        {
            _context.Set<UserMultiAccount>().Attach(userMultiAccount);
            return _context.SaveChangesAsync();
        }
    }
}
