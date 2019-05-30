using System;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.PayMatrixLevel
{
    public class PayMatrixLevelCommand : IRequest<string>
    {
        public Guid UserMultiAccountId { get; set; }
        public int MatrixLevel { get; set; }
        public decimal Amount { get; set; }
    }
}
