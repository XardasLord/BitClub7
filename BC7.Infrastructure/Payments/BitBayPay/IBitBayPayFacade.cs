using System;
using System.Threading.Tasks;
using BC7.Infrastructure.Payments.BitBayPay.BodyModels;

namespace BC7.Infrastructure.Payments.BitBayPay
{
    public interface IBitBayPayFacade
    {
        Task<CreatePaymentResponse> CreatePayment(Guid orderId, decimal price);
    }
}
