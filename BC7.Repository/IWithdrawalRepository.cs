using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Domain;

namespace BC7.Repository
{
    public interface IWithdrawalRepository
    {
        Task<Withdrawal> GetAsync(Guid id);
        Task<List<Withdrawal>> GetAllAsync();
        Task<List<Withdrawal>> GetAllAsync(IEnumerable<Guid> userMultiAccountIds);
        Task CreateAsync(Withdrawal withdrawal);
        Task UpdateAsync(Withdrawal withdrawal);
    }
}