using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BC7.Api.Controllers
{
    public class MatrixPositionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatrixPositionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}

// TODO: MatrixPositionsController -> api/matrixPositions/{userMultiAccountId} (query: ?matrixLevel=0) [POST]