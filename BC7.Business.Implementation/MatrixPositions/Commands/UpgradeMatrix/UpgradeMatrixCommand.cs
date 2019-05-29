using System;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix
{
    public class UpgradeMatrixCommand : IRequest<UpgradeMatrixResult>
    {
        public int MatrixLevel { get; }
        public Guid UserMultiAccountId { get; }

        public UpgradeMatrixCommand(int matrixLevel, Guid userMultiAccountId)
        {
            MatrixLevel = matrixLevel;
            UserMultiAccountId = userMultiAccountId;
        }
    }
}
