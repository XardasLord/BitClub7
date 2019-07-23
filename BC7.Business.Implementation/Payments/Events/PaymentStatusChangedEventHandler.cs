using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Implementation.Withdrawals.Jobs;
using BC7.Business.Models;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using Hangfire;
using MediatR;

namespace BC7.Business.Implementation.Payments.Events
{
    public class PaymentStatusChangedEventHandler : INotificationHandler<PaymentStatusChangedEvent>
    {
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;

        public PaymentStatusChangedEventHandler(
            IPaymentHistoryRepository paymentHistoryRepository, 
            IUserAccountDataRepository userAccountDataRepository,
            IUserMultiAccountRepository userMultiAccountRepository,
            IBackgroundJobClient backgroundJobClient)
        {
            _userAccountDataRepository = userAccountDataRepository;
            _userMultiAccountRepository = userMultiAccountRepository;
            _backgroundJobClient = backgroundJobClient;
            _paymentHistoryRepository = paymentHistoryRepository;
        }

        public async Task Handle(PaymentStatusChangedEvent notification, CancellationToken cancellationToken = default(CancellationToken))
        {
            var paymentHistory = await _paymentHistoryRepository.GetAsync(notification.PaymentId);

            paymentHistory.ChangeStatus(notification.Status);
            paymentHistory.Paid(notification.PaidAmount);

            await _paymentHistoryRepository.UpdateAsync(paymentHistory);
            await NotifyProperPaymentChange(notification.OrderId, notification.PaymentId, paymentHistory.PaymentFor);
        }

        private Task NotifyProperPaymentChange(Guid orderId, Guid paymentId, string paymentFor)
        {
            switch (paymentFor)
            {
                case "MembershipsFee": // TODO: Maybe it should be enum for the simplicity? If yes then migration is necessary to change the PaymentFor table in domain
                    return MembershipsFeePaid(orderId);
                case "MatrixLevel0":
                case "MatrixLevel1":
                case "MatrixLevel2":
                case "MatrixLevel3":
                case "MatrixLevel4":
                case "MatrixLevel5":
                case "MatrixLevel6":
                    return Task.CompletedTask;
                case "ProjectDonation":
                    return ProjectDonationPaid(orderId, paymentId);
                default:
                    throw new ValidationException($"Unknown paymentFor value: {paymentFor}");
            }
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

        private async Task ProjectDonationPaid(Guid orderId, Guid paymentId)
        {
            var multiAccount = await _userMultiAccountRepository.GetAsync(orderId);
            if (multiAccount is null)
            {
                throw new ValidationException($"Cannot find the MultiAccount from PaymentHistory with OrderId: {orderId}");
            }

            var payment = await _paymentHistoryRepository.GetAsync(paymentId);

            var projectDonatedModel = new ProjectDonatedModel
            {
                DonatedUserMultiAccountId = orderId,
                Amount = payment.AmountToPay
            };

            _backgroundJobClient.Enqueue<ProjectDonatedJob>(
                job => job.Execute(projectDonatedModel, null));
        }
    }
}
