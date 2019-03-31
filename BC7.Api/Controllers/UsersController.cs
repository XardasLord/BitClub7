﻿using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Implementation.Authentications.Commands.RegisterNewUserAccount;
using BC7.Business.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BC7.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Register new user account
        /// </summary>
        /// <param name="model">A model with all user account data</param>
        /// <param name="reflink">Optional query parameter. If send it will attach the main account to this reflink's user</param>
        /// <returns>Returns the Id of the created user account</returns>
        /// <response code="200">Returns the Id of the newly created user account</response>
        [HttpPost]
        public async Task<IActionResult> RegisterNewAccount([FromBody] RegisterNewUserModel model, string reflink = null)
        {
            var command = _mapper.Map<RegisterNewUserAccountCommand>(model);
            command.InvitingRefLink = reflink;

            var resultId = await _mediator.Send(command);

            // TODO: Created (201) maybe?
            return Ok(new { Id = resultId });
        }
    }
}
