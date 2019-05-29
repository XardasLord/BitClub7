using System;
using System.Threading.Tasks;
using BC7.Database;
using BC7.Domain;
using Microsoft.EntityFrameworkCore;

namespace BC7.Repository.Implementation
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IBitClub7Context _context;

        public TicketRepository(IBitClub7Context context)
        {
            _context = context;
        }

        public Task<Ticket> GetAsync(Guid id)
        {
            return _context.Set<Ticket>().SingleAsync(x => x.Id == id);
        }

        public Task CreateAsync(Ticket ticket)
        {
            _context.Set<Ticket>().Add(ticket);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Ticket ticket)
        {
            _context.Set<Ticket>().Attach(ticket);
            return _context.SaveChangesAsync();
        }
    }
}
