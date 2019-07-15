using System.Threading.Tasks;
using BC7.Business.Implementation.Withdrawals.Requests.GetWithdrawals;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BC7.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WithdrawalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WithdrawalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all withdrawals
        /// </summary>
        /// <returns>Returns model with list of withdrawals</returns>
        /// <response code="200">Success - returns model with list of withdrawals</response>
        /// <response code="403">Fail - only root users have access</response>
        [HttpGet]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetWithdrawalsRequest()));
        }
    }
}
