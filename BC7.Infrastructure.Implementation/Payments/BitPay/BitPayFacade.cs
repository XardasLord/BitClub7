using System;
using System.Threading.Tasks;
using BC7.Common.Settings;
using BC7.Infrastructure.Payments.BitBayPay.BodyModels;
using BC7.Infrastructure.Payments.BitPay;
using BitPaySDK;
using BitPaySDK.Models.Invoice;
using Microsoft.Extensions.Options;

namespace BC7.Infrastructure.Implementation.Payments.BitPay
{
    public class BitPayFacade : IBitPayFacade
    {
        private readonly IOptions<BitPayApiSettings> _bitPayApiSettings;

        public BitPayFacade(IOptions<BitPayApiSettings> bitPayApiSettings)
        {
            _bitPayApiSettings = bitPayApiSettings;
        }

        public async Task<CreatePaymentResponse> CreatePayment(Guid orderId, decimal price)
        {
            var bitpay = new BitPaySDK.BitPay("BitPay.config.json");
            //var bitpay = new BitPaySDK.BitPay(
            //    Env.Test,
            //    "",
            //    new Env.Tokens()
            //    {
            //        POS = "AvJdGrEqTW9HVbqaQDhWHRacHYgfgxsJit9zabAnrJaK",
            //        Merchant = "CE2WRSEExNFA4YdQyyDJmgVAot9FgXvXbo752TGA7eUj",
            //        Payout = "9pJ7fzW1GGeucVMcDrs7HDQfj32aNATCDnyY6YAaHUNo"
            //    }
            //);

            var invoiceModel = new Invoice
            {
                Price = (double) price,
                Currency = _bitPayApiSettings.Value.DestinationCurrency,
                Buyer = new Buyer { },
                FullNotifications = true,
                NotificationEmail = "satoshi@example.com"
            };

            var invoice = await bitpay.CreateInvoice(invoiceModel);

            throw new NotImplementedException();
        }
    }
}
