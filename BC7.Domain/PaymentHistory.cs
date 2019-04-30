using System;
using BC7.Common.Extensions;
using BC7.Infrastructure.CustomExceptions;

namespace BC7.Domain
{
    public static class PaymentForHelper
    {
        public static readonly string MembershipsFee = "MembershipsFee";
        public static readonly string MatrixLevel0Position = "MatrixLevel0Position";
    }

    public static class PaymentStatusHelper
    {
        public static readonly string NotPaid = "NOT PAID";
        public static readonly string Paid = "PAID";
        public static readonly string Completed = "COMPLETED";
    }

    public class PaymentHistory
    {
        public Guid Id { get; private set; }
        public Guid PaymentId { get; private set; }
        public Guid OrderId { get; private set; }
        public double AmountToPay { get; private set; }
        public double PaidAmount { get; private set; }
        public string Status { get; private set; }
        public string PaymentFor { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private PaymentHistory()
        {
        }

        public PaymentHistory(Guid id, Guid paymentId, Guid orderId, double amountToPay, string paymentFor)
        {
            ValidateDomain(id, paymentId, orderId, amountToPay, paymentFor);

            Id = id;
            PaymentId = paymentId;
            OrderId = orderId;
            AmountToPay = amountToPay;
            PaidAmount = 0;
            Status = PaymentStatusHelper.NotPaid;
            PaymentFor = paymentFor;
            CreatedAt = DateTime.UtcNow;
        }

        private static void ValidateDomain(Guid id, Guid paymentId, Guid orderId, double amountToPay, string paymentFor)
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

            if (paymentFor.IsNullOrWhiteSpace())
            {
                throw new DomainException("PaymentFor value is null or empty");
            }
        }

        public void ChangeStatus(string newStatus)
        {
            if (Status == PaymentStatusHelper.Completed && newStatus == PaymentStatusHelper.Paid)
            {
                throw new DomainException($"Cannot change the status {Status} to {newStatus}");
            }

            Status = newStatus;
        }

        public void Paid(double paidAmount)
        {
            PaidAmount += paidAmount;
        }
    }
}
