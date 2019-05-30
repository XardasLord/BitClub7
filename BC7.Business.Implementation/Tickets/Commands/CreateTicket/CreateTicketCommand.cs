using System;
using MediatR;

namespace BC7.Business.Implementation.Tickets.Commands.CreateTicket
{
    public class CreateTicketCommand : IRequest<Guid>
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
    }
}
