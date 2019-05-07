using System.Threading.Tasks;
using BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix;
using BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix;
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

        /// <summary>
        /// Buy position in matrix
        /// </summary>
        /// <param name="command">Command with matrix level where user would like to buy a position and a userMultiAccountId who wants to buy a position in matrix</param>
        /// <returns>Returns the Id of the bought matrix position</returns>
        /// <response code="200">Returns the ID of the bought matrix position</response>
        [HttpPost("buyPosition")]
        [Authorize]
        public async Task<IActionResult> BuyPosition([FromBody] BuyPositionInMatrixCommand command)
        {
            // TODO: status 201 maybe?
            return Ok(new { Id = await _mediator.Send(command) });
        }

        /// <summary>
        /// Upgrade matrix position
        /// </summary>
        /// <param name="command">Command with matrix level to upgrade and a userMultiAccountId who wants to upgrade his matrix position</param>
        /// <returns>Returns the ID of the position in higher level upgraded matrix</returns>
        [HttpPost("upgrade")]
        [Authorize]
        public async Task<IActionResult> UpgradeMatrixLevel([FromBody] UpgradeMatrixCommand command)
        {
            // TODO: status 201 maybe?
            return Ok(new { Id = await _mediator.Send(command) });
        }
    }
}