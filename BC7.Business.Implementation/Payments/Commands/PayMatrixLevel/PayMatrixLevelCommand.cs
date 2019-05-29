using System;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.PayMatrixLevel
{
    public class PayMatrixLevelCommand : IRequest<string>
    {
        public Guid UserMultiAccountId { get; }
        public int MatrixLevel { get; }
        public decimal Amount { get; }

        public PayMatrixLevelCommand(Guid userMultiAccountId, int matrixLevel, decimal amount)
        {
            UserMultiAccountId = userMultiAccountId;
            MatrixLevel = matrixLevel;
            Amount = amount;
        }
    }
}
