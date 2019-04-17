using System;
using System.Threading.Tasks;
using BC7.Infrastructure.Payments;

namespace BC7.Infrastructure.Implementation.Payments
{
    public class BitBayPayFacade : IBitBayPayFacade
    {
        public BitBayPayFacade()
        {
        }

        public Task<string> CreatePayment()
        {
            throw new NotImplementedException();
        }
    }
}
