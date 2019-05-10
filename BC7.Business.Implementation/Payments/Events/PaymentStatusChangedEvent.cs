using System;
using MediatR;

namespace BC7.Business.Implementation.Payments.Events
{
    public class PaymentStatusChangedEvent : INotification
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public decimal? AmountToPayInSourceCurrency { get; set; }
        public decimal AmountToPayInDestinationCurrency { get; set; }
        public string Status { get; set; } // PAID / COMPLETED
        public decimal? PaidAmount { get; set; }
    }
}
