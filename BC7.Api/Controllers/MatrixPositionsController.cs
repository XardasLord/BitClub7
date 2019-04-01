using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BC7.Api.Controllers
{
    [Route("api/[controller]")]
    public class MatrixPositionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatrixPositionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{matrixLevel}/{userMultiAccountId}")]
        [Authorize]
        public Task<IActionResult> BuyPosition(int matrixLevel, Guid userMultiAccountId)
        {
            throw new NotImplementedException();
        }
    }
}