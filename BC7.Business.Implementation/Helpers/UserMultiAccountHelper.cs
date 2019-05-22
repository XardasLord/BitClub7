using System;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Database;
using BC7.Domain;
using BC7.Repository;
using Microsoft.EntityFrameworkCore;

namespace BC7.Business.Implementation.Helpers
{
    public class UserMultiAccountHelper : IUserMultiAccountHelper
    {
        private readonly IBitClub7Context _context;
        private readonly IUserAccountDataRepository _userAccountDataRepository;

        public UserMultiAccountHelper(IBitClub7Context context, IUserAccountDataRepository userAccountDataRepository)
        {
            _context = context;
            _userAccountDataRepository = userAccountDataRepository;
        }

        public Task<UserMultiAccount> GetRandomUserMultiAccount()
        {
            return _context.Set<UserMultiAccount>()
                .OrderBy(r => Guid.NewGuid())
                .Take(1)
                .FirstAsync();
        }

        public async Task<string> GenerateNextMultiAccountName(Guid userAccountDataId)
        {
            var userAccount = await _userAccountDataRepository.GetAsync(userAccountDataId);

            var numberOfMultiAccounts = userAccount.UserMultiAccounts.Count;
            if (numberOfMultiAccounts == 0)
            {
                return userAccount.Login;
            }

            return $"{userAccount.Login}-{numberOfMultiAccounts:000}";
        }
    }
}
