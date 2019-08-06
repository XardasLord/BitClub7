using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Payments.Events;
using BC7.Business.Implementation.Withdrawals.Jobs;
using BC7.Business.Models;
using BC7.Infrastructure.CustomExceptions;
using BC7.Infrastructure.Implementation.Hangfire;
using BC7.Repository;
using Hangfire;
using Hangfire.Server;

namespace BC7.Business.Implementation.Jobs
{
    public class PaymentStatusChangedEventJob : IJob<PaymentStatusChangedEvent>
    {
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IPaymentHistoryRepository _paymentHistoryRepository;

        public PaymentStatusChangedEventJob(
            IPaymentHistoryRepository paymentHistoryRepository,
            IUserAccountDataRepository userAccountDataRepository,
            IBackgroundJobClient backgroundJobClient)
        {
            _userAccountDataRepository = userAccountDataRepository;
            _backgroundJobClient = backgroundJobClient;
            _paymentHistoryRepository = paymentHistoryRepository;
        }

        public async Task Execute(PaymentStatusChangedEvent @event, PerformContext context)
        {
            var paymentHistory = await _paymentHistoryRepository.GetAsync(@event.PaymentId);

            paymentHistory.ChangeStatus(@event.Status);
            paymentHistory.Paid(@event.PaidAmount);

            await _paymentHistoryRepository.UpdateAsync(paymentHistory);
            await NotifyProperPaymentChange(@event.OrderId, @event.PaymentId, paymentHistory.PaymentFor);
        }

        private async Task NotifyProperPaymentChange(Guid orderId, Guid paymentId, string paymentFor)
        {
            switch (paymentFor)
            {
                case "MembershipsFee": // TODO: Maybe it should be enum for the simplicity? If yes then migration is necessary to change the PaymentFor table in domain
                    await MembershipsFeePaid(orderId);
                    break;
                case "MatrixLevel0":
                case "MatrixLevel1":
                case "MatrixLevel2":
                case "MatrixLevel3":
                case "MatrixLevel4":
                case "MatrixLevel5":
                case "MatrixLevel6": // TODO: Auto-upgrade on status change
                    break;
                case "DonationForFoundation":
                    await DonationForFoundationPaid(paymentId);
                    break;
                case "ProjectDonation":
                    await ProjectDonationPaid(paymentId);
                    break;
                case "ProjectDonationViaAffiliateProgram":
                    await ProjectDonationViaAffiliateProgramPaid(paymentId);
                    break;
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

        private async Task DonationForFoundationPaid(Guid paymentId)
        {
            var payment = await _paymentHistoryRepository.GetAsync(paymentId);

            var projectDonatedModel = new DonationForFoundationModel
            {
                DonatedUserMultiAccountId = payment.UserPaymentForId,
                Amount = payment.AmountToPay
            };

            _backgroundJobClient.Enqueue<DonationForFoundationJob>(
                job => job.Execute(projectDonatedModel, null));
        }

        private async Task ProjectDonationPaid(Guid paymentId)
        {
            var payment = await _paymentHistoryRepository.GetAsync(paymentId);

            var projectDonatedModel = new ProjectDonatedModel
            {
                DonatedUserMultiAccountId = payment.UserPaymentForId,
                Amount = payment.AmountToPay
            };

            _backgroundJobClient.Enqueue<ProjectDonatedJob>(
                job => job.Execute(projectDonatedModel, null));
        }

        private async Task ProjectDonationViaAffiliateProgramPaid(Guid paymentId)
        {
            var payment = await _paymentHistoryRepository.GetAsync(paymentId);

            var projectDonatedViaAffiliateProgramModel = new ProjectDonatedModelViaAffiliateProgramModel
            {
                DonatedUserMultiAccountId = payment.UserPaymentForId,
                Amount = payment.AmountToPay
            };

            _backgroundJobClient.Enqueue<ProjectDonatedViaAffiliateProgramJob>(
                job => job.Execute(projectDonatedViaAffiliateProgramModel, null));
        }
    }
}