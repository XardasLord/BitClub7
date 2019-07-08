using System;
using System.Collections.Generic;
using BC7.Business.Models;
using MediatR;

namespace BC7.Business.Implementation.Users.Requests.GetPaymentHistories
{
    public class GetPaymentHistoriesRequest : IRequest<IEnumerable<PaymentHistoryModel>>
    {
        public Guid UserAccountId { get; set; }
    }
}