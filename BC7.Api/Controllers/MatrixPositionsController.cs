using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix;
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
        public async Task<IActionResult> BuyPosition(int matrixLevel, Guid userMultiAccountId)
        {
            var command = new BuyPositionInMatrixCommand
            {
                UserMultiAccountId = userMultiAccountId,
                MatrixLevel = matrixLevel
            };

            // TODO: status 201 maybe?
            return Ok(new { Id = await _mediator.Send(command) });
        }
    }
}