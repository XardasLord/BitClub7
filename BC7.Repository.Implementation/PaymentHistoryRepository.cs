using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<List<PaymentHistory>> GetAllAsync()
        {
            return _context.Set<PaymentHistory>().ToListAsync();
        }

        public Task<PaymentHistory> GetAsync(Guid paymentId)
        {
            return _context.Set<PaymentHistory>().SingleAsync(x => x.PaymentId == paymentId);
        }

        public Task<List<PaymentHistory>> GetPaymentsByUser(Guid userId)
        {
            return _context.Set<PaymentHistory>()
                .Where(x => x.OrderId == userId)
                .ToListAsync();
        }

        public Task CreateAsync(PaymentHistory paymentHistory)
        {
            _context.Set<PaymentHistory>().Add(paymentHistory);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(PaymentHistory paymentHistory)
        {
            _context.Set<PaymentHistory>().Attach(paymentHistory);
            return _context.SaveChangesAsync();
        }
    }
}
