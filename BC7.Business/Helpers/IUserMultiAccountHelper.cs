using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Domain;

namespace BC7.Business.Helpers
{
    public interface IUserMultiAccountHelper
    {
        Task<UserMultiAccount> GetRandomUserMultiAccountSponsor();
        Task<string> GenerateNextMultiAccountName(Guid userAccountDataId);
        Task<List<UserMultiAccount>> GetAllWhereMultiAccountIsSponsor(Guid userMultiAccountId);
        Task<List<UserMultiAccount>> GetAllWhereMultiAccountsAreSponsors(IEnumerable<Guid> userMultiAccountIds);
    }
}
