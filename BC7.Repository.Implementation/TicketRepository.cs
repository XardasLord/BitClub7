using System.Threading.Tasks;
using BC7.Database;
using BC7.Domain;

namespace BC7.Repository.Implementation
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IBitClub7Context _context;

        public TicketRepository(IBitClub7Context context)
        {
            _context = context;
        }

        public Task CreateAsync(Ticket ticket)
        {
            _context.Set<Ticket>().Add(ticket);
            return _context.SaveChangesAsync();
        }
    }
}
