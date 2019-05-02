using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.PayMatrixLevel
{
    public class PayMatrixLevelCommandHandler : IRequestHandler<PayMatrixLevelCommand, string>
    {
        public PayMatrixLevelCommandHandler()
        {
        }

        public Task<string> Handle(PayMatrixLevelCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
