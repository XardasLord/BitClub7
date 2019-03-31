using System;
using System.Threading.Tasks;
using BC7.Entity;

namespace BC7.Business.Helpers
{
    public interface IUserAccountDataHelper
    {
        Task<UserAccountData> GetById(Guid id);
    }
}
