using System.Threading.Tasks;
using BC7.Business.Implementation.Payments.Commands.PayMembershipsFee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BC7.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Pay the membership's fee
        /// </summary>
        /// <param name="command">A command with user main account ID and amount to pay</param>
        /// <returns>Returns the Url where the payment can be done</returns>
        /// <response code="200">Returns the Url where the payment can be done</response>
        [HttpPost("membershipsFee")]
        [Authorize]
        public async Task<IActionResult> PayMembershipsFee([FromBody] PayMembershipsFeeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}