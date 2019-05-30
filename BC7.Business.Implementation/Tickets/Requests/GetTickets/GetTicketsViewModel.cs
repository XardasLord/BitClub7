using System.Collections.Generic;
using BC7.Business.Models;

namespace BC7.Business.Implementation.Tickets.Requests.GetTickets
{
    public class GetTicketsViewModel
    {
        public IEnumerable<TicketModel> TicketModels { get; set; }
        // TODO: Pagination etc...
    }
}