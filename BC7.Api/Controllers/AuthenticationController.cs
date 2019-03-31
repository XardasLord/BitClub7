﻿using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Implementation.Authentications.Commands.Login;
using BC7.Business.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BC7.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var command = _mapper.Map<LoginCommand>(model);

            var token = await _mediator.Send(command);

            return Ok(new { Token = token });
        }
    }
}
