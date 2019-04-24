using System;
using BC7.Infrastructure.CustomExceptions;

namespace BC7.Domain
{
    public class PaymentHistory
    {
        public Guid Id { get; private set; }
        public Guid PaymentId { get; private set; }
        public Guid OrderId { get; private set; }
        public double AmountToPay { get; private set; }
        public double PaidAmount { get; private set; }
        public string Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private PaymentHistory()
        {
        }

        public PaymentHistory(Guid id, Guid paymentId, Guid orderId, double amountToPay)
        {
            ValidateDomain(id, paymentId, orderId, amountToPay);

            Id = id;
            PaymentId = paymentId;
            OrderId = orderId;
            AmountToPay = amountToPay;
            PaidAmount = 0;
            Status = "NOT PAID"; // todo enum or some string constraints probably would be better (NOT PAID, PAID, COMPLETED)
            CreatedAt = DateTime.UtcNow;
        }

        private static void ValidateDomain(Guid id, Guid paymentId, Guid orderId, double amountToPay)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException("Invalid ID.");
            }
            if (paymentId == Guid.Empty)
            {
                throw new DomainException("Invalid paymentId.");
            }
            if (orderId == Guid.Empty)
            {
                throw new DomainException("Invalid orderId.");
            }
            if (amountToPay <= 0)
            {
                throw new DomainException($"Invalid amountToPay value: {amountToPay}");
            }
        }
    }
}
