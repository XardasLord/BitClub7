namespace BC7.Common.Settings
{
    public class BitBayPayApiSettings
    {
        public string ApiUrl { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string DestinationCurrency { get; set; }
        public double MembershipsFeeAmount { get; set; }
        public string NotificationsUrl { get; set; }
    }
}
