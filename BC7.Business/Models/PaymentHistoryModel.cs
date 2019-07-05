using System;

namespace BC7.Business.Models
{
    public class PaymentHistoryModel
    {
        public Guid PaymentId { get; set; }
        public string AccountName { get; set; }
        public string Status { get; set; }
        public string PaymentFor { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}