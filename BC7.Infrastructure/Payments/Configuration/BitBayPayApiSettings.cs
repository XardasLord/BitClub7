namespace BC7.Infrastructure.Payments.Configuration
{
    public interface IBitBayPayApiSettings
    {
        string ApiUrl { get; }
        string PublicKey { get; }
        string PrivateKey { get; }
    }
}
