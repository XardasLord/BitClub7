using System;
using System.Threading.Tasks;
using BC7.Domain;

namespace BC7.Repository
{
    public interface IUserMultiAccountRepository
    {
        Task<UserMultiAccount> GetAsync(Guid id);
        Task<UserMultiAccount> GetByReflinkAsync(string reflink);
        Task<UserMultiAccount> GetByAccountNameAsync(string accountName);
        Task CreateAsync(UserMultiAccount userMultiAccount);
    }
}
