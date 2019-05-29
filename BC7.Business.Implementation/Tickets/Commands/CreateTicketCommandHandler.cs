using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Domain;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Tickets.Commands
{
    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
    {
        private readonly ITicketRepository _ticketRepository;

        public CreateTicketCommandHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Guid> Handle(CreateTicketCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ticket = new Ticket(Guid.NewGuid(), command.Email, command.Subject, command.Text);

            await _ticketRepository.CreateAsync(ticket);

            // TODO: Event `TicketCreated` to change the ticket full number - `ticket-000001`

            return ticket.Id;
        }
    }
}