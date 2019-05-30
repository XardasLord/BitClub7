using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Business.Implementation.Tickets.Requests.GetTickets;
using FluentAssertions;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.Ticket
{
    [Story(
        AsA = "As a user",
        IWant = "I want to get all tickets",
        SoThat = "So all tickets are returned"
    )]
    public class GetTicketsTest : BaseIntegration
    {
        private GetTicketsRequest _request;
        private GetTicketsRequestHandler _sut;
        private GetTicketsViewModel _result;

        void GivenSystemUnderTest()
        {
            _sut = new GetTicketsRequestHandler(_ticketRepository, _mapper);
        }

        async Task AndGivenCreatedTicketsInDatabase()
        {
            var fakerGenerator = new FakerGenerator();
            
            var tickets = fakerGenerator.GetTicketsFakerGenerator()
                .Generate(5);

            _context.Tickets.AddRange(tickets);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _request = new GetTicketsRequest();
        }

        async Task WhenHandlerHandlesTheCommand()
        {
            _result = await _sut.Handle(_request);
        }

        void ThenResultHasAllTicketsFromDatabase()
        {
            _result.TicketModels.Count().Should().Be(5);
        }

        [Test]
        public void GetAllTickets()
        {
            this.BDDfy();
        }
    }
}
