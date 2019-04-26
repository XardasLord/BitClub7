using System;
using System.Threading.Tasks;
using BC7.Domain;

namespace BC7.Repository
{
    public interface IPaymentHistoryRepository
    {
        Task<PaymentHistory> GetAsync(Guid paymentId);
        Task CreateAsync(PaymentHistory paymentHistory);
    }
}
