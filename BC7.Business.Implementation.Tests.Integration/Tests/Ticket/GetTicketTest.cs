using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Business.Implementation.Tickets.Requests.GetTicket;
using BC7.Business.Models;
using FluentAssertions;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.Ticket
{
    [Story(
        AsA = "As a root user",
        IWant = "I want to get a specific ticket by ID",
        SoThat = "So the requested ticket is returned"
    )]
    public class GetTicketTest : BaseIntegration
    {
        private GetTicketRequest _request;
        private GetTicketRequestHandler _sut;
        private TicketModel _result;
        private readonly Guid _ticketId = Guid.NewGuid();

        void GivenSystemUnderTest()
        {
            _sut = new GetTicketRequestHandler(_ticketRepository, _mapper);
        }

        async Task AndGivenCreatedTicketsInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var testedTicket = fakerGenerator.GetTicketsFakerGenerator()
                .RuleFor(x => x.Id, _ticketId)
                .RuleFor(x => x.FullTicketNumber, "ticket-000005")
                .Generate();

            var tickets = fakerGenerator.GetTicketsFakerGenerator()
                .Generate(5);

            tickets.Add(testedTicket);

            _context.Tickets.AddRange(tickets);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _request = new GetTicketRequest
            {
                Id = _ticketId
            };
        }

        async Task WhenHandlerHandlesTheCommand()
        {
            _result = await _sut.Handle(_request);
        }

        void ThenResultIsRequestedTicketFromDatabase()
        {
            _result.TicketNumber.Should().Be("ticket-000005");
        }

        [Test]
        public void GetTicketById()
        {
            this.BDDfy();
        }
    }
}
