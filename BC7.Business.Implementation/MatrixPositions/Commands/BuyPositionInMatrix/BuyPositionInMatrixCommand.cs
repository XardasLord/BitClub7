using System;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix
{
    public class BuyPositionInMatrixCommand : IRequest<Guid>
    {
        public Guid UserMultiAccountId { get; }
        public int MatrixLevel { get; }

        public BuyPositionInMatrixCommand(Guid userMultiAccountId, int matrixLevel)
        {
            UserMultiAccountId = userMultiAccountId;
            MatrixLevel = matrixLevel;
        }
    }
}
