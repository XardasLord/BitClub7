using System.Threading.Tasks;
using BC7.Business.Implementation.Authentications.Commands.Login;
using BC7.Business.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Log in user to the system
        /// </summary>
        /// <param name="model">A model with credentials to log in</param>
        /// <returns>Returns a JWT token</returns>
        /// <response code="200">Returns a JWT token</response>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var command = new LoginCommand { LoginOrEmail = model.LoginOrEmail, Password = model.Password };

            var token = await _mediator.Send(command);

            return Ok(new { Token = token });
        }
    }
}
