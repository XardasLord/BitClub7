using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Payments.Events;
using BC7.Infrastructure.CustomExceptions;
using BC7.Infrastructure.Implementation.Hangfire;
using BC7.Repository;
using Hangfire.Server;

namespace BC7.Business.Implementation.Jobs
{
    public class PaymentStatusChangedEventJob : IJob<PaymentStatusChangedEvent>
    {
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;

        public PaymentStatusChangedEventJob(
            IPaymentHistoryRepository paymentHistoryRepository,
            IUserAccountDataRepository userAccountDataRepository,
            IUserMultiAccountRepository userMultiAccountRepository)
        {
            _userAccountDataRepository = userAccountDataRepository;
            _userMultiAccountRepository = userMultiAccountRepository;
            _paymentHistoryRepository = paymentHistoryRepository;
        }

        public async Task Execute(PaymentStatusChangedEvent @event, PerformContext context)
        {
            var paymentHistory = await _paymentHistoryRepository.GetAsync(@event.PaymentId);

            paymentHistory.ChangeStatus(@event.Status);
            paymentHistory.Paid(@event.PaidAmount);

            await _paymentHistoryRepository.UpdateAsync(paymentHistory);
            await NotifyProperPaymentChange(@event.OrderId, paymentHistory.PaymentFor);
        }

        private Task NotifyProperPaymentChange(Guid orderId, string paymentFor)
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
    }
}