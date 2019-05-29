using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Implementation.Events;
using BC7.Domain;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Tickets.Commands
{
    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMediator _mediator;

        public CreateTicketCommandHandler(ITicketRepository ticketRepository, IMediator mediator)
        {
            _ticketRepository = ticketRepository;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(CreateTicketCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ticket = new Ticket(Guid.NewGuid(), command.Email, command.Subject, command.Text);

            await _ticketRepository.CreateAsync(ticket);

            var @event = new TicketCreatedEvent(ticket.Id);
            await _mediator.Publish(@event);

            return ticket.Id;
        }
    }
}