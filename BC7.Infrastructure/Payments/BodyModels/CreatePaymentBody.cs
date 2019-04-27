using System;
using Newtonsoft.Json;

namespace BC7.Infrastructure.Payments.BodyModels
{
    public class CreatePaymentBody
    {
        [JsonProperty(PropertyName = "destinationCurrency")]
        public string DestinationCurrency { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "orderId")]
        public Guid OrderId { get; set; }

        [JsonProperty(PropertyName = "coveredBy")]
        public string CoveredBy { get; set; }

        [JsonProperty(PropertyName = "keepSourceCurrency")]
        public bool KeepSourceCurrency { get; set; }

        [JsonProperty(PropertyName = "sourceCurrency")]
        public string SourceCurrency { get; set; }

        [JsonProperty(PropertyName = "successCallbackUrl")]
        public string SuccessCallbackUrl { get; set; }

        [JsonProperty(PropertyName = "failureCallbackUrl")]
        public string FailureCallbackUrl { get; set; }

        [JsonProperty(PropertyName = "notificationsUrl")]
        public string NotificationsUrl { get; set; }
    }
}
