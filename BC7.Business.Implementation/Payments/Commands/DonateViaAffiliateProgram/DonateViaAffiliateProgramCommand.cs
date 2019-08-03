using System;
using MediatR;

namespace BC7.Business.Implementation.Payments.Commands.DonateViaAffiliateProgram
{
    public class DonateViaAffiliateProgramCommand : IRequest<DonateViaAffiliateProgramViewModel>
    {
        public Guid UserMultiAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}