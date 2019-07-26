using System;
using System.Threading.Tasks;
using BC7.Infrastructure.Payments.BitBayPay.BodyModels;

namespace BC7.Infrastructure.Payments.BitPay
{
    public interface IBitPayFacade
    {
        Task<CreatePaymentResponse> CreatePayment(Guid orderId, decimal price);
    }
}