using System;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Database;
using BC7.Entity;
using BC7.Repository;
using Microsoft.EntityFrameworkCore;

namespace BC7.Business.Implementation.Helpers
{
    public class UserMultiAccountHelper : IUserMultiAccountHelper
    {
        private readonly IBitClub7Context _context;
        private readonly IReflinkHelper _reflinkHelper;
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;

        public UserMultiAccountHelper(IBitClub7Context context, IReflinkHelper reflinkHelper, IUserMultiAccountRepository userMultiAccountRepository)
        {
            _context = context;
            _reflinkHelper = reflinkHelper;
            _userMultiAccountRepository = userMultiAccountRepository;
        }

        public Task<UserMultiAccount> GetRandomUserMultiAccount()
        {
            return _context.Set<UserMultiAccount>()
                .OrderBy(r => Guid.NewGuid())
                .Take(1)
                .FirstAsync();
        }

#warning Move to repository
        public async Task<UserMultiAccount> Create(Guid userAccountId, string reflink)
        {
            var userMultiAccountInviting = await _userMultiAccountRepository.GetByReflinkAsync(reflink);
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
