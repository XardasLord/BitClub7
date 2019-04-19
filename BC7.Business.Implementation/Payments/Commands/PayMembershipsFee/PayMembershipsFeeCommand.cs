using System;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.PayMembershipsFee
{
    public class PayMembershipsFeeCommand : IRequest
    {
        public Guid UserAccountDataId { get; set; }
        public double Amount { get; set; }
    }
}
