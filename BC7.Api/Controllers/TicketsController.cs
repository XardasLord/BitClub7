using System.Threading.Tasks;
using BC7.Business.Implementation.Tickets.Commands.CreateTicket;
using BC7.Business.Implementation.Tickets.Requests.GetTickets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BC7.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all tickets
        /// </summary>
        /// <returns>Returns model with list of tickets</returns>
        /// <response code="200">Success - returns model with list of tickets</response>
        /// <response code="403">Fail - only root users have access</response>
        [HttpGet]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetTicketsRequest()));
        }

        /// <summary>
        /// Create new ticket
        /// </summary>
        /// <param name="command">Command with email, subject and text</param>
        /// <returns>Returns ID of the newly created ticket</returns>
        /// <response code="200">Success - returns ID of the newly created ticket</response>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateTicketCommand command)
        {
            var ticketId = await _mediator.Send(command);
            return Ok(new { Id = ticketId });
        }
    }
}