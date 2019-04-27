using System;
using System.Threading.Tasks;
using BC7.Database;
using BC7.Domain;
using Microsoft.EntityFrameworkCore;

namespace BC7.Repository.Implementation
{
    public class PaymentHistoryRepository : IPaymentHistoryRepository
    {
        private readonly IBitClub7Context _context;

        public PaymentHistoryRepository(IBitClub7Context context)
        {
            _context = context;
        }

        public Task<PaymentHistory> GetAsync(Guid paymentId)
        {
            return _context.Set<PaymentHistory>().SingleOrDefaultAsync(x => x.PaymentId == paymentId);
        }

        public async Task CreateAsync(PaymentHistory paymentHistory)
        {
            await _context.Set<PaymentHistory>().AddAsync(paymentHistory);
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(PaymentHistory paymentHistory)
        {
            _context.Set<PaymentHistory>().Attach(paymentHistory);
            return _context.SaveChangesAsync();
        }
    }
}
