using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.MultiAccounts.Commands.CreateMultiAccount;
using BC7.Business.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BC7.Api.Controllers
{
    public class MultiAccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MultiAccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
