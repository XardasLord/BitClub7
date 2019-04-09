using System;
using System.Threading.Tasks;
using BC7.Entity;

namespace BC7.Business.Helpers
{
    public interface IUserMultiAccountHelper
    {
        Task<UserMultiAccount> GetRandomUserMultiAccount();
        Task<string> GenerateNextMultiAccountName(Guid userAccountDataId);
    }
}
