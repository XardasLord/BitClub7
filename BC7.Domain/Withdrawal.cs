using System;
using BC7.Domain.Enums;
using BC7.Infrastructure.CustomExceptions;

namespace BC7.Domain
{
    public class Withdrawal
    {
        public Guid Id { get; private set; }
        public Guid? WithdrawalId { get; private set; }
        public Guid UserMultiAccountId { get; private set; }
        public virtual UserMultiAccount UserMultiAccount { get; private set; }

        public decimal Amount { get; private set; }
        public PaymentSystemType PaymentSystemType { get; private set; }
        public DateTimeOffset? WithdrawnAt { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public Withdrawal(Guid id, Guid userMultiAccountId, decimal amount, PaymentSystemType paymentSystemType)
        {
            ValidateDomain(id, userMultiAccountId, amount);

            Id = id;
            UserMultiAccountId = userMultiAccountId;
            Amount = amount;
            PaymentSystemType = paymentSystemType;
            WithdrawalId = null;
            WithdrawnAt = null;
            CreatedAt = DateTimeOffset.Now;
        }

        private static void ValidateDomain(Guid id, Guid userMultiAccountId, decimal amount)
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