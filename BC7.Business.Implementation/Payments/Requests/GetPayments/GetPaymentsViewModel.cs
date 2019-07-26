using System.Collections.Generic;
using BC7.Business.Models;

namespace BC7.Business.Implementation.Payments.Requests.GetPayments
{
    public class GetPaymentsViewModel
    {
        public IEnumerable<PaymentHistoryModel> PaymentHistoryModel { get; set; }
        public int TotalCount { get; set; }
    }
}