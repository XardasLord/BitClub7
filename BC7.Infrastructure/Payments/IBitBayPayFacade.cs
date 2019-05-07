using System;
using System.Threading.Tasks;
using BC7.Infrastructure.Payments.BodyModels;

namespace BC7.Infrastructure.Payments
{
    public interface IBitBayPayFacade
    {
        Task<CreatePaymentResponse> CreatePayment(Guid orderId, decimal price);
    }
}
