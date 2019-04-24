using System;
using MediatR;

namespace BC7.Business.Implementation.Payments.Events
{
    public class PaymentStatusChangedEvent : INotification
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public double AmountToPayInSourceCurrency { get; set; }
        public double AmountToPayInDestinationCurrency { get; set; }
        public string Status { get; set; } // PAID / COMPLETED
        public double PaidAmount { get; set; }
    }
}
