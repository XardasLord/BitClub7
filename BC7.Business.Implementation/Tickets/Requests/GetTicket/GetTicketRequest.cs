using System;
using BC7.Business.Models;
using MediatR;

namespace BC7.Business.Implementation.Tickets.Requests.GetTicket
{
    public class GetTicketRequest : IRequest<TicketModel>
    {
        public Guid Id { get; set; }
    }
}
