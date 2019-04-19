using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BC7.Infrastructure.Payments;
using BC7.Infrastructure.Payments.BodyModels;
using BC7.Infrastructure.Payments.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace BC7.Infrastructure.Implementation.Payments
{
    public class BitBayPayFacade : IBitBayPayFacade
    {
        private readonly IOptions<BitBayPayApiSettings> _bitBayPayApiSettings;

        public BitBayPayFacade(IOptions<BitBayPayApiSettings> bitBayPayApiSettings)
        {
            _bitBayPayApiSettings = bitBayPayApiSettings;
        }

        public Task<string> CreatePayment(Guid orderId, double price)
        {
            var client = new RestClient(_bitBayPayApiSettings.Value.ApiUrl);
            var request = new RestRequest("payments", Method.POST);

            var createPaymentBody = new CreatePaymentBody()
            {
                DestinationCurrency = "btc", // TODO: Enums?
                Price = price,
                OrderId = orderId
            };

            var body = SerializeObjectToJsonString(createPaymentBody);
            var unixTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds; // TODO: Helper?
            var apiHash = GenerateApiHash(_bitBayPayApiSettings.Value.PublicKey, unixTimestamp, _bitBayPayApiSettings.Value.PrivateKey, body);

            request.AddHeader("API-Key", _bitBayPayApiSettings.Value.PublicKey);
            request.AddHeader("API-Hash", apiHash);
            request.AddHeader("operation-id", Guid.NewGuid().ToString());
            request.AddHeader("Request-Timestamp", unixTimestamp.ToString());
            request.AddHeader("Content-Type", "application/json");

            request.AddJsonBody(createPaymentBody);

            var response = client.Execute(request);

            throw new NotImplementedException();
        }

        private string GenerateApiHash(string publicKey, long unixTimestamp, string privateKey, string bodyJson)
        {
            // TODO: Move to helper
            var hash = new StringBuilder();
            var key = publicKey + unixTimestamp + bodyJson;
            
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(privateKey)))
            {
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
                foreach (var theByte in hmac.Hash)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        private string SerializeObjectToJsonString(object obj)
        {
            // TODO: Move to helper
            return JsonConvert.SerializeObject(obj, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}
