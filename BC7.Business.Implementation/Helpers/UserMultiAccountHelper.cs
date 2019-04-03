using System;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Database;
using BC7.Entity;
using Microsoft.EntityFrameworkCore;

namespace BC7.Business.Implementation.Helpers
{
    public class UserMultiAccountHelper : IUserMultiAccountHelper
    {
        private readonly IBitClub7Context _context;
        private readonly IReflinkHelper _reflinkHelper;

        public UserMultiAccountHelper(IBitClub7Context context, IReflinkHelper reflinkHelper)
        {
            _context = context;
            _reflinkHelper = reflinkHelper;
        }

        public Task<UserMultiAccount> GetById(Guid id)
        {
            return _context.Set<UserMultiAccount>()
                .Include(x => x.MatrixPositions)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task<UserMultiAccount> GetByReflink(string reflink)
        {
            return _context.Set<UserMultiAccount>().SingleOrDefaultAsync(x => x.RefLink == reflink);
        }

        public Task<UserMultiAccount> GetByAccountName(string accountName)
        {
            return _context.Set<UserMultiAccount>().SingleOrDefaultAsync(x => x.MultiAccountName == accountName);
        }

        public Task<UserMultiAccount> GetRandomUserMultiAccount()
        {
            return _context.Set<UserMultiAccount>()
                .OrderBy(r => Guid.NewGuid())
                .Take(1)
                .FirstAsync();
        }

        public async Task<UserMultiAccount> Create(Guid userAccountId, string reflink)
        {
            var userMultiAccountInviting = await GetByReflink(reflink);
            var multiAccountName = await GenerateNextMultiAccountName(userAccountId);

            var userMultiAccount = new UserMultiAccount
            {
                UserAccountDataId = userAccountId,
                UserMultiAccountInvitingId = userMultiAccountInviting.Id,
                MultiAccountName = multiAccountName,
                RefLink = _reflinkHelper.GenerateReflink(),
                IsMainAccount = false
            };

            await _context.Set<UserMultiAccount>().AddAsync(userMultiAccount);
            await _context.SaveChangesAsync();

            return userMultiAccount;
        }

        public async Task<string> GenerateNextMultiAccountName(Guid userAccountDataId)
        {
            var userAccount = await _context.Set<UserAccountData>()
                .Include(x => x.UserMultiAccounts)
                .Where(x => x.Id == userAccountDataId)
                .SingleAsync();

            var numberOfMultiAccounts = userAccount.UserMultiAccounts.Count;

            if (numberOfMultiAccounts == 0)
            {
                return userAccount.Login;
            }

            return $"{userAccount.Login}-{numberOfMultiAccounts:000}";
        }
    }
}
