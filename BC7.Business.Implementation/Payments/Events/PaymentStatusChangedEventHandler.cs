using System.Threading;
using System.Threading.Tasks;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Payments.Events
{
    public class PaymentStatusChangedEventHandler : INotificationHandler<PaymentStatusChangedEvent>
    {
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;

        public PaymentStatusChangedEventHandler(IPaymentHistoryRepository paymentHistoryRepository)
        {
            _paymentHistoryRepository = paymentHistoryRepository;
        }

        public async Task Handle(PaymentStatusChangedEvent notification, CancellationToken cancellationToken = default(CancellationToken))
        {
            var paymentHistory = await _paymentHistoryRepository.GetAsync(notification.PaymentId);

            paymentHistory.ChangeStatus(notification.Status);
            paymentHistory.Paid(notification.PaidAmount);

            await _paymentHistoryRepository.UpdateAsync(paymentHistory);
        }
    }
}
