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
        /// TODO
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("membershipsFee")]
        [Authorize]
        public async Task<IActionResult> PayMembershipsFee([FromBody] PayMembershipsFeeCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}