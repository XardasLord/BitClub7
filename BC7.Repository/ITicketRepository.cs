using System.Threading.Tasks;
using BC7.Domain;

namespace BC7.Repository
{
    public interface ITicketRepository
    {
        Task CreateAsync(Ticket ticket);
    }
}
