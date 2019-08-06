using System;
using BC7.Common.Extensions;
using BC7.Domain.Enums;
using BC7.Infrastructure.CustomExceptions;

namespace BC7.Domain
{
    public static class WithdrawalForHelper
    {
	    public static readonly string DonationForFoundation = "Donation for foundation";
		public static readonly string ProjectDonation = "Project donation";
        public static readonly string ProjectDonationViaAffiliateProgramBc7ConstFee = "Project donation via affiliate program (9.5%)";
        public static readonly string ProjectDonationViaAffiliateProgramBc7DonateFee = "Project donation via affiliate program (5%)";
        public static readonly string ProjectDonationViaAffiliateProgramDirectDonate = "Project donation via affiliate program (Direct donate 80%)";
        public static readonly string ProjectDonationViaAffiliateProgramLineA = "Project donation via affiliate program (Line A 10%)";
        public static readonly string ProjectDonationViaAffiliateProgramLineB = "Project donation via affiliate program (Line B 5%)";
        public static readonly string AssignmentInMatrix = "Assignment in matrix";
        public static readonly string UpgradedMatrix = "Upgraded matrix";
    }

    public class Withdrawal
    {
        public Guid Id { get; private set; }
        public Guid? WithdrawalId { get; private set; }
        public Guid UserMultiAccountId { get; private set; }
        public virtual UserMultiAccount UserMultiAccount { get; private set; }

        public decimal Amount { get; private set; }
        public string WithdrawalFor { get; private set; }
        public PaymentSystemType PaymentSystemType { get; private set; }
        public DateTimeOffset? WithdrawnAt { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public Withdrawal(Guid id, Guid userMultiAccountId, decimal amount, PaymentSystemType paymentSystemType, string withdrawalFor)
        {
            ValidateDomain(id, userMultiAccountId, amount, withdrawalFor);

            Id = id;
            UserMultiAccountId = userMultiAccountId;
            Amount = amount;
            PaymentSystemType = paymentSystemType;
            WithdrawalFor = withdrawalFor;
            WithdrawalId = null;
            WithdrawnAt = null;
            CreatedAt = DateTimeOffset.Now;
        }

        private static void ValidateDomain(Guid id, Guid userMultiAccountId, decimal amount, string withdrawalFor)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException("Invalid withdrawal ID.");
            }
            if (userMultiAccountId == Guid.Empty)
            {
                throw new DomainException("Invalid userMultiAccount ID.");
            }
            if (amount <= 0)
            {
                throw new DomainException($"Invalid amount value: {amount}");
            }
            if (withdrawalFor.IsNullOrWhiteSpace())
            {
                throw new DomainException("Invalid withdrawalFor value.");
            }
        }

        public void Withdraw(Guid withdrawalId)
        {
            if (WithdrawnAt.HasValue || WithdrawalId.HasValue)
            {
                throw new DomainException($"Withdrawal has already been done at {WithdrawnAt} with withdrawal ID: {WithdrawalId}.");
            }
            if (withdrawalId == Guid.Empty)
            {
                throw new ValidationException("WithdrawalId parameter cannot be empty.");
            }

            WithdrawalId = withdrawalId;
            WithdrawnAt = DateTimeOffset.Now;
        }
    }
}