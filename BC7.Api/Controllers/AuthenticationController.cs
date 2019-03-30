using System.Threading.Tasks;
using BC7.Business.Implementation.Authentications.Commands.RegisterNewUserAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BC7.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("registerNewAccount")]
        public async Task<IActionResult> RegisterNewAccount([FromBody] RegisterNewUserAccountCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
