using System;
using MediatR;

namespace BC7.Business.Implementation.Events
{
    public class TicketCreatedEvent : INotification
    {
        public Guid TicketId { get; }

        public TicketCreatedEvent(Guid ticketId)
        {
            TicketId = ticketId;
        }
    }
}
