using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Payments.Events
{
    public class PaymentStatusChangedEventHandler : INotificationHandler<PaymentStatusChangedEvent>
    {
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;

        public PaymentStatusChangedEventHandler(IPaymentHistoryRepository paymentHistoryRepository, IUserAccountDataRepository userAccountDataRepository)
        {
            _userAccountDataRepository = userAccountDataRepository;
            _paymentHistoryRepository = paymentHistoryRepository;
        }

        public async Task Handle(PaymentStatusChangedEvent notification, CancellationToken cancellationToken = default(CancellationToken))
        {
            var paymentHistory = await _paymentHistoryRepository.GetAsync(notification.PaymentId);

            paymentHistory.ChangeStatus(notification.Status);
            paymentHistory.Paid(notification.PaidAmount);

            await _paymentHistoryRepository.UpdateAsync(paymentHistory);
            await NotifyProperPaymentChange(notification.OrderId, paymentHistory.PaymentFor);
        }

        private Task NotifyProperPaymentChange(Guid orderId, string paymentFor)
        {
            switch (paymentFor)
            {
                case "MembershipsFee": // TODO: Maybe it should be enum for the simplicity? If yes then migration is necessary to change the PaymentFor table in domain
                    return MembershipsFeePaid(orderId);
                    //TODO: Other types in the future
            }

            throw new ValidationException($"Unknown paymentFor value: {paymentFor}");
        }

        private async Task MembershipsFeePaid(Guid orderId)
        {
            var userAccount = await _userAccountDataRepository.GetAsync(orderId);
            if (userAccount is null)
            {
                throw new ValidationException($"Cannot find the UserAccountData from PaymentHistory with OrderId: {orderId}");
            }

            userAccount.PaidMembershipFee();
            await _userAccountDataRepository.UpdateAsync(userAccount);
        }
    }
}
