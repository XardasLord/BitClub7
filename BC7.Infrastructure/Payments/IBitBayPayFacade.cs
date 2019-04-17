using System;
using System.Threading.Tasks;

namespace BC7.Infrastructure.Payments
{
    public interface IBitBayPayFacade
    {
        Task<string> CreatePayment(Guid orderId, double price); // TODO: parameters will be given during the development
    }
}
