using System.Threading.Tasks;
using BC7.Business.Implementation.Jobs;
using BC7.Business.Implementation.Payments.Commands.Donate;
using BC7.Business.Implementation.Payments.Commands.PayMatrixLevel;
using BC7.Business.Implementation.Payments.Commands.PayMembershipsFee;
using BC7.Business.Implementation.Payments.Events;
using BC7.Business.Implementation.Payments.Requests.GetPayments;
using Hangfire;
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
        private readonly IBackgroundJobClient _backgroundJobClient;

        public PaymentsController(IMediator mediator, IBackgroundJobClient backgroundJobClient)
        {
            _mediator = mediator;
            _backgroundJobClient = backgroundJobClient;
        }

        /// <summary>
        /// Get all payments registered in the system
        /// </summary>
        /// <returns>Returns list of whole payments</returns>
        /// <response code="200">Returns list of whole payments</response>
        /// <response code="401">Failed - only logged in users have access</response>
        /// <response code="403">Fail - only root users have access</response>
        [HttpGet]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetPaymentsRequest()));
        }

        /// <summary>
        /// Pay the membership's fee
        /// </summary>
        /// <param name="command">A command with user main account ID and amount to pay</param>
        /// <returns>Returns the Url where the payment can be done</returns>
        /// <response code="200">Returns the Url where the payment can be done</response>
        /// <response code="401">Failed - only logged in users have access</response>
        [HttpPost("membershipsFee")]
        [Authorize]
        public async Task<IActionResult> PayMembershipsFee([FromBody] PayMembershipsFeeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Pay for the matrix entrance on given level
        /// </summary>
        /// <param name="command">A command with user main account ID and amount to pay and a matrix level to buy entrance to</param>
        /// <returns>Returns the Url where the payment can be done</returns>
        /// <response code="200">Returns the Url where the payment can be done</response>
        /// <response code="401">Failed - only logged in users have access</response>
        [HttpPost("matrixLevel")]
        [Authorize]
        public async Task<IActionResult> PayForMatrixLevel([FromBody] PayMatrixLevelCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Donation to the project
        /// </summary>
        /// <param name="command">A command with project and amount to donate</param>
        /// <returns>Returns the Url where the payment can be done</returns>
        /// <response code="200">Returns the Url where the payment can be done</response>
        [HttpPost("donation")]
        public async Task<IActionResult> Donate([FromBody] DonateCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Notification directly from BitBayPay system that payment status has changed
        /// </summary>
        /// <param name="event">Event with parameters to inform the system that payment's status has changed</param>
        /// <returns>Nothing</returns>
        /// <response code="200">Nothing</response>
        [HttpPost("paymentNotification")]
        [AllowAnonymous]
        public IActionResult PaymentNotification([FromBody] PaymentStatusChangedEvent @event)
        {
            _backgroundJobClient.Enqueue<PaymentStatusChangedEventJob>(
                job => job.Execute(@event, null));

            return Ok();
        }
    }
}