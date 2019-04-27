using System;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.PayMembershipsFee
{
    public class PayMembershipsFeeCommand : IRequest<string>
    {
        public Guid UserAccountDataId { get; set; }
        public double Amount { get; set; }
    }
}
