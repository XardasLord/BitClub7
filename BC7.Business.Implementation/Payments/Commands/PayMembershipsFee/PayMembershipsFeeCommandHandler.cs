using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Infrastructure.Payments;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.PayMembershipsFee
{
    public class PayMembershipsFeeCommandHandler : IRequestHandler<PayMembershipsFeeCommand>
    {
        private readonly IBitBayPayFacade _bitBayPayFacade;

        public PayMembershipsFeeCommandHandler(IBitBayPayFacade bitBayPayFacade)
        {
            _bitBayPayFacade = bitBayPayFacade;
        }

        public Task<Unit> Handle(PayMembershipsFeeCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var orderId = Guid.NewGuid();

            //TODO: Tmp store orderId in DB with UserAccountDataId

            _bitBayPayFacade.CreatePayment(orderId, command.Amount);

            //TODO: Logic after payment call

            throw new NotImplementedException();
        }
    }
}