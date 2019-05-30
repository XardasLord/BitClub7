using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Models;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Tickets.Requests.GetTicket
{
    public class GetTicketRequestHandler : IRequestHandler<GetTicketRequest, TicketModel>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public GetTicketRequestHandler(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<TicketModel> Handle(GetTicketRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var ticket = await _ticketRepository.GetAsync(request.Id);

            return _mapper.Map<TicketModel>(ticket);
        }
    }
}