using System;
using System.Threading.Tasks;
using BC7.Domain;

namespace BC7.Repository
{
    public interface ITicketRepository
    {
        Task<Ticket> GetAsync(Guid id);
        Task CreateAsync(Ticket ticket);
        Task UpdateAsync(Ticket ticket);
    }
}
