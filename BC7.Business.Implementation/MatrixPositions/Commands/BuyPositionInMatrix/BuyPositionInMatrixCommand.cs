using System;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix
{
    public class BuyPositionInMatrixCommand : IRequest<Guid>
    {
        public Guid UserMultiAccountId { get; set; }
        public int MatrixLevel { get; set; }
    }
}
