using System;
using System.Collections.Generic;
using BC7.Common.Extensions;
using BC7.Infrastructure.CustomExceptions;

namespace BC7.Domain
{
    public static class PaymentForHelper
    {
        public static readonly string MembershipsFee = "MembershipsFee";
        public static readonly string ProjectDonation = "ProjectDonation";
        public static readonly string DonationForFoundation = "DonationForFoundation";
		public static readonly string ProjectDonationViaAffiliateProgram = "ProjectDonationViaAffiliateProgram";
        public static readonly Dictionary<int, string> MatrixLevelPositionsDictionary = new Dictionary<int, string>
        {
            {0, "MatrixLevel0"},
            {1, "MatrixLevel1"},
            {2, "MatrixLevel2"},
            {3, "MatrixLevel3"},
            {4, "MatrixLevel4"},
            {5, "MatrixLevel5"},
            {6, "MatrixLevel6"},
        };
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

		/// <summary>
		/// ID of the payment in the external payment system
		/// </summary>
        public Guid PaymentId { get; private set; }

		/// <summary>
		/// User who makes payment
		/// </summary>
        public Guid OrderId { get; private set; }

		/// <summary>
		/// User for whom payment is done
		/// </summary>
        public Guid UserPaymentForId { get; private set; }
		public decimal AmountToPay { get; private set; }
        public decimal? PaidAmount { get; private set; }
        public string Status { get; private set; }
        public string PaymentFor { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private PaymentHistory()
        {
        }

        public PaymentHistory(Guid id, Guid paymentId, Guid orderId, Guid userPaymentForId, decimal amountToPay, string paymentFor)
        {
            ValidateDomain(id, paymentId, orderId, userPaymentForId, amountToPay, paymentFor);

            Id = id;
            PaymentId = paymentId;
            OrderId = orderId;
            UserPaymentForId = userPaymentForId;
            AmountToPay = amountToPay;
            PaidAmount = null;
            Status = PaymentStatusHelper.NotPaid;
            PaymentFor = paymentFor;
            CreatedAt = DateTime.UtcNow;
        }

        private static void ValidateDomain(Guid id, Guid paymentId, Guid orderId, Guid userPaymentForId, decimal amountToPay, string paymentFor)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException("Invalid ID.");
            }
            if (paymentId == Guid.Empty)
            {
                throw new DomainException($"Invalid {nameof(paymentId)}.");
            }
            if (orderId == Guid.Empty)
            {
                throw new DomainException($"Invalid {nameof(orderId)}.");
			}
			if (userPaymentForId == Guid.Empty)
			{
				throw new DomainException($"Invalid {nameof(userPaymentForId)}.");
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

        public void Paid(decimal? paidAmount)
        {
            PaidAmount = paidAmount;
        }
    }
}
