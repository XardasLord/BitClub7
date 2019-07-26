using System;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrixWithoutPaymentValidation_TMP
{
    public class BuyPositionInMatrixWithoutPaymentValidation_TMPCommand : IRequest<Guid>
    {
        public Guid UserMultiAccountId { get; set; }
        public int MatrixLevel { get; set; }
    }
}