using System.Threading.Tasks;
using BC7.Database;
using BC7.Domain;

namespace BC7.Repository.Implementation
{
    public class PaymentHistoryRepository : IPaymentHistoryRepository
    {
        private readonly IBitClub7Context _context;

        public PaymentHistoryRepository(IBitClub7Context context)
        {
            _context = context;
        }

        public async Task CreateAsync(PaymentHistory paymentHistory)
        {
            await _context.Set<PaymentHistory>().AddAsync(paymentHistory);
            await _context.SaveChangesAsync();
        }
    }
}
