using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Domain;

namespace BC7.Repository
{
    public interface IUserAccountDataRepository
    {
        Task<List<UserAccountData>> GetAllAsync();
        Task<UserAccountData> GetAsync(Guid id);
        Task<UserAccountData> GetAsync(string emailOrLogin);
        Task UpdateAsync(UserAccountData userAccountData);
    }
}
