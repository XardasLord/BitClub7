using System;
using System.Threading.Tasks;
using BC7.Database;
using BC7.Entity;
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
                .Include(x => x.MatrixPositions)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        // TODO: Some BrowseAsync with Func/Expression maybe?
        public Task<UserMultiAccount> GetByReflinkAsync(string reflink)
        {
            return _context.Set<UserMultiAccount>().SingleOrDefaultAsync(x => x.RefLink == reflink);
        }
    }
}
