using System;
using System.Threading.Tasks;
using BC7.Domain;

namespace BC7.Business.Helpers
{
    public interface IUserMultiAccountHelper
    {
        Task<UserMultiAccount> GetRandomUserMultiAccountSponsor();
        Task<string> GenerateNextMultiAccountName(Guid userAccountDataId);
    }
}
