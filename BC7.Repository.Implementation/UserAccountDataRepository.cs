using System;
using System.Threading.Tasks;
using BC7.Entity;

namespace BC7.Repository.Implementation
{
    public class UserAccountDataRepository : IUserAccountDataRepository
    {
        public Task<UserAccountData> Get(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
