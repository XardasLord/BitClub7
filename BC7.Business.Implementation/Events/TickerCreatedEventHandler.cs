using System.Threading;
using System.Threading.Tasks;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Events
{
    class TickerCreatedEventHandler : INotificationHandler<TicketCreatedEvent>
    {
        private readonly ITicketRepository _ticketRepository;

        public TickerCreatedEventHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task Handle(TicketCreatedEvent notification, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ticket = await _ticketRepository.GetAsync(notification.TicketId);

            ticket.UpdateTicketNumber();

            await _ticketRepository.UpdateAsync(ticket);
        }
    }
}
