using System;
using BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrixWithoutPaymentValidation_TMP
{
    public class UpgradeMatrixWithoutPaymentValidation_TMPCommand : IRequest<UpgradeMatrixResult>
    {
        public int MatrixLevel { get; set; }
        public Guid UserMultiAccountId { get; set; }
    }
}