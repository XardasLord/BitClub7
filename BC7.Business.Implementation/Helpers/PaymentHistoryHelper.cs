using System;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Domain;
using BC7.Repository;

namespace BC7.Business.Implementation.Helpers
{
    public class PaymentHistoryHelper : IPaymentHistoryHelper
    {
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;

        public PaymentHistoryHelper(IPaymentHistoryRepository paymentHistoryRepository)
        {
            _paymentHistoryRepository = paymentHistoryRepository;
        }

        public async Task<bool> DoesUserPaidForMatrixLevelAsync(int matrixLevel, Guid userMultiAccountId)
        {
            var userPayments = await _paymentHistoryRepository.GetPaymentsByUser(userMultiAccountId);

            return userPayments.Any(
                x => x.PaymentFor == PaymentForHelper.MatrixLevelPositionsDictionary[matrixLevel] &&
                     (x.Status == PaymentStatusHelper.Paid || x.Status == PaymentStatusHelper.Completed)
            );
        }
    }
}
