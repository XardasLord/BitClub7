using System;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.PayMembershipsFee
{
    public class PayMembershipsFeeCommand : IRequest<string>
    {
        public Guid UserAccountDataId { get; }
        public decimal Amount { get; }

        public PayMembershipsFeeCommand(Guid userAccountDataId, decimal amount)
        {
            UserAccountDataId = userAccountDataId;
            Amount = amount;
        }
    }
}
