using System;
using System.Threading.Tasks;

namespace BC7.Business.Helpers
{
    public interface IUserMultiAccountHelper
    {
        Task<string> GetNextMultiAccountName(Guid userAccountDataId);
    }
}
