using System;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Database;
using BC7.Entity;
using Microsoft.EntityFrameworkCore;

namespace BC7.Business.Implementation.Helpers
{
    public class UserAccountDataHelper : IUserAccountDataHelper
    {
        private readonly IBitClub7Context _context;

        public UserAccountDataHelper(IBitClub7Context context)
        {
            _context = context;
        }

        public Task<UserAccountData> GetById(Guid id)
        {
            return _context.Set<UserAccountData>()
                .Include(x => x.UserMultiAccounts)
                .ThenInclude(x => x.MatrixPositions)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
