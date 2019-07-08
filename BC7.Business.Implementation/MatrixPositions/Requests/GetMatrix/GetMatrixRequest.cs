using System;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Requests.GetMatrix
{
    public class GetMatrixRequest : IRequest<MatrixViewModel>
    {
        public Guid MatrixPositionId { get; set; }
    }
}
