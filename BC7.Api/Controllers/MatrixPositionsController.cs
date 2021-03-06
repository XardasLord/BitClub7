﻿using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix;
using BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrixWithoutPaymentValidation_TMP;
using BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix;
using BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrixWithoutPaymentValidation_TMP;
using BC7.Business.Implementation.MatrixPositions.Requests.GenerateTreeDefinitionFile;
using BC7.Business.Implementation.MatrixPositions.Requests.GetMatrix;
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
        /// <response code="401">Failed - only logged in users have access</response>
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
        /// <response code="200">Returns the ID of the position in higher level upgraded matrix</response>
        /// <response code="401">Failed - only logged in users have access</response>
        [HttpPost("upgrade")]
        [Authorize]
        public async Task<IActionResult> UpgradeMatrixLevel([FromBody] UpgradeMatrixCommand command)
        {
            // TODO: status 201 maybe?
            return Ok(new { Id = await _mediator.Send(command) });
        }

        /// <summary>
        /// Get matrix for given MatrixPositionId
        /// </summary>
        /// <param name="matrixPositionId">ID of the position to get its matrix</param>
        /// <returns>Returns matrix for given matrix position ID</returns>
        /// <response code="200">Returns matrix for given matrix position ID</response>
        /// <response code="401">Failed - only logged in users have access</response>
        [HttpGet("{matrixPositionId}/matrix")]
        [Authorize]
        public async Task<IActionResult> GetMatrix(Guid matrixPositionId)
        {
            var request = new GetMatrixRequest { MatrixPositionId = matrixPositionId };
            return Ok(await _mediator.Send(request));
        }

        /// <summary>
        /// Helper endpoint to generate the matrix tree structure - it saves the tree definition file which can be drawn using the Graphviz library
        /// </summary>
        /// <param name="matrixLevel">Matrix level</param>
        /// <returns>Saves the file on a disc</returns>
        /// <response code="403">Failed - only root users have access</response>
        [HttpGet("treeStructure/{matrixLevel}")]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> GenerateTreeDefinitionFile(int matrixLevel)
        {
            await _mediator.Send(new GenerateTreeDefinitionFileRequest(matrixLevel));
            return Ok();
        }
        

        // THESE TWO ARE ONLY FOR TMP INIT WACH'S ACCOUNTS IN MATRIX
        [HttpPost("upgradeMatrixWithoutPaymentValidation_TMP")]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> UpgradeMatrixWithoutPaymentValidation_TMP([FromBody] UpgradeMatrixWithoutPaymentValidation_TMPCommand command)
        {
            return Ok(new { Id = await _mediator.Send(command) });
        }

        [HttpPost("buyMatrixPositionWithoutPaymentValidation_TMP")]
        [Authorize(Roles = "Root")]
        public async Task<IActionResult> BuyMatrixPositionWithoutPaymentValidation_TMP([FromBody] BuyPositionInMatrixWithoutPaymentValidation_TMPCommand command)
        {
            return Ok(new { Id = await _mediator.Send(command) });
        }
    }
}