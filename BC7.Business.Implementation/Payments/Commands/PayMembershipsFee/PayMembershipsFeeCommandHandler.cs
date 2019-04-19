using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.PayMembershipsFee
{
    public class PayMembershipsFeeCommandHandler : IRequestHandler<PayMembershipsFeeCommand>
    {
        public PayMembershipsFeeCommandHandler()
        {
        }

        public Task<Unit> Handle(PayMembershipsFeeCommand request, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}