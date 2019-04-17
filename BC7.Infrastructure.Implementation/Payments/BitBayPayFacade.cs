using System;
using System.Threading.Tasks;
using BC7.Infrastructure.Payments;
using BC7.Infrastructure.Payments.Configuration;
using Microsoft.Extensions.Options;

namespace BC7.Infrastructure.Implementation.Payments
{
    public class BitBayPayFacade : IBitBayPayFacade
    {
        private readonly IOptions<BitBayPayApiSettings> _bitBayPayApiSettings;

        public BitBayPayFacade(IOptions<BitBayPayApiSettings> bitBayPayApiSettings)
        {
            _bitBayPayApiSettings = bitBayPayApiSettings;
        }

        public Task<string> CreatePayment()
        {
            throw new NotImplementedException();
        }
    }
}
