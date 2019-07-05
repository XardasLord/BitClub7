using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Domain;

namespace BC7.Repository
{
    public interface IPaymentHistoryRepository
    {
        Task<PaymentHistory> GetAsync(Guid paymentId);
        Task<List<PaymentHistory>> GetPaymentsByUser(Guid userId);
        Task CreateAsync(PaymentHistory paymentHistory);
        Task UpdateAsync(PaymentHistory paymentHistory);
    }
}
