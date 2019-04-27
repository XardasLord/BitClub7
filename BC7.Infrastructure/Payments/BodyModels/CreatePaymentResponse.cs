using System;

namespace BC7.Infrastructure.Payments.BodyModels
{
    public class CreatePaymentResponse
    {
        public CreatePaymentData Data { get; set; }
        public string Status { get; set; }
    }

    public class CreatePaymentData
    {
        public Guid PaymentId { get; set; }
        public string Url { get; set; }
    }
}
