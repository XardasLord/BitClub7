using System.Threading.Tasks;
using BC7.Business.Implementation.Tickets.Commands;
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