using System.Threading.Tasks;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tickets.Commands;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.Ticket
{
    [Story(
        AsA = "As a user",
        IWant = "I want to create a ticket",
        SoThat = "So the new ticket is created"
    )]
    public class CreateTicketTest : BaseIntegration
    {
        private CreateTicketCommand _command;
        private CreateTicketCommandHandler _sut;

        void GivenSystemUnderTest()
        {
            _sut = new CreateTicketCommandHandler(_ticketRepository);
        }

        void AndGivenCommandPrepared()
        {
            _command = new CreateTicketCommand
            {
                Email = "test@email.com",
                Subject = "Example subject",
                Text = "Example text"
            };
        }

        async Task WhenHandlerHandlesTheCommand()
        {
            await _sut.Handle(_command);
        }

        async Task ThenTicketExistsInDatabaseWithAppropriateNumber()
        {
            var ticket = await _context.Set<Domain.Ticket>().SingleAsync();

            ticket.Number.Should().Be(1);
            ticket.Email.Should().Be("test@email.com");
            ticket.Subject.Should().Be("Example subject");
            ticket.Text.Should().Be("Example text");
        }

        [Test]
        public void CreateNewTicket()
        {
            this.BDDfy();
        }
    }
}
