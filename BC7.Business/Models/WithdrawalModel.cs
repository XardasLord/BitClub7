using System;

namespace BC7.Business.Models
{
    public class WithdrawalModel
    {
        public Guid Id { get; set; }
        public Guid UserAccountDataId { get; set; }
        public Guid UserMultiAccountId { get; set; }
        public decimal Amount { get; set; }
        public string MultiAccountName { get; set; }
        public string BtcWalletAddress { get; set; }
        public string PaymentFor { get; set; }
        public bool IsWithdrawn { get; set; }
        public DateTimeOffset? WithdrawnAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}