using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Models;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Tickets.Requests.GetTickets
{
    public class GetTicketsRequestHandler : IRequestHandler<GetTicketsRequest, GetTicketsViewModel>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public GetTicketsRequestHandler(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<GetTicketsViewModel> Handle(GetTicketsRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var tickets = await _ticketRepository.GetAllAsync();

            var ticketModels = _mapper.Map<IEnumerable<TicketModel>>(tickets);

            return new GetTicketsViewModel
            {
                TicketModels = ticketModels
            };
        }
    }
}