using System;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix
{
    public class UpgradeMatrixCommand : IRequest<UpgradeMatrixResult>
    {
        public int MatrixLevel { get; set; }
        public Guid UserMultiAccountId { get; set; }
    }
}
