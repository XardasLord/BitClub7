using System;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Business.Models;
using BC7.Domain;
using BC7.Domain.Enums;
using BC7.Infrastructure.Implementation.Hangfire;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Newtonsoft.Json;

namespace BC7.Business.Implementation.Withdrawals.Jobs
{
    public class ProjectDonatedViaAffiliateProgramJob : IJob<ProjectDonatedModelViaAffiliateProgramModel>
    {
        private readonly Guid _rootId = Guid.Parse("441C799C-E2B7-4F1C-B141-DB3C6C1AF034"); // TODO: Move this to the settings

        private readonly IWithdrawalHelper _withdrawalHelper;
        private readonly IUserMultiAccountHelper _userMultiAccountHelper;

        public ProjectDonatedViaAffiliateProgramJob(IWithdrawalHelper withdrawalHelper, IUserMultiAccountHelper userMultiAccountHelper)
        {
            _withdrawalHelper = withdrawalHelper;
            _userMultiAccountHelper = userMultiAccountHelper;
        }

        public async Task Execute(ProjectDonatedModelViaAffiliateProgramModel model, PerformContext context)
        {
            context.WriteLine($"ProjectDonatedViaAffiliateProgramJob started with model data - {JsonConvert.SerializeObject(model)}");
            
            // BC7Fee (9.5%)
            var bc7Fee = _withdrawalHelper.CalculateAmountToWithdraw(model.Amount, AffiliateProgramType.Bc7ConstFee);
            var bc7ConstWithdrawal = new Withdrawal(
                Guid.NewGuid(),
                _rootId,
                bc7Fee,
                PaymentSystemType.BitBayPay,
                WithdrawalForHelper.ProjectDonationViaAffiliateProgramBc7ConstFee);

            BackgroundJob.Enqueue<CommitWithdrawalJob>(
                job => job.Execute(bc7ConstWithdrawal, null));
            
            var amountAfterTakenFee = model.Amount - bc7Fee;


            // DonatedDirectly (80%)
            var directDonateWithdrawal = new Withdrawal(
                Guid.NewGuid(),
                model.DonatedUserMultiAccountId,
                _withdrawalHelper.CalculateAmountToWithdraw(amountAfterTakenFee, AffiliateProgramType.DirectDonate),
                PaymentSystemType.BitBayPay,
                WithdrawalForHelper.ProjectDonationViaAffiliateProgramDirectDonate);

            BackgroundJob.Enqueue<CommitWithdrawalJob>(
                job => job.Execute(directDonateWithdrawal, null));


            // DonatedLineA (10%)
            var lineAMultiAccount = await _userMultiAccountHelper.GetSponsorForMultiAccount(model.DonatedUserMultiAccountId);
            var lineADonateWithdrawal = new Withdrawal(
                Guid.NewGuid(),
                lineAMultiAccount.Id,
                _withdrawalHelper.CalculateAmountToWithdraw(amountAfterTakenFee, AffiliateProgramType.AffiliateLineA),
                PaymentSystemType.BitBayPay,
                WithdrawalForHelper.ProjectDonationViaAffiliateProgramLineA);

            BackgroundJob.Enqueue<CommitWithdrawalJob>(
                job => job.Execute(lineADonateWithdrawal, null));


            // DonatedLineB (5%)
            var lineBMultiAccount = await _userMultiAccountHelper.GetSponsorForMultiAccount(lineAMultiAccount.Id);
            var lineBDonateWithdrawal = new Withdrawal(
                Guid.NewGuid(),
                lineBMultiAccount.Id,
                _withdrawalHelper.CalculateAmountToWithdraw(amountAfterTakenFee, AffiliateProgramType.AffiliateLineB),
                PaymentSystemType.BitBayPay,
                WithdrawalForHelper.ProjectDonationViaAffiliateProgramLineB);

            BackgroundJob.Enqueue<CommitWithdrawalJob>(
                job => job.Execute(lineBDonateWithdrawal, null));


            // BC7DonatedFee (5%)
            var bc7DonateFeeWithdrawal = new Withdrawal(
                Guid.NewGuid(),
                _rootId,
                _withdrawalHelper.CalculateAmountToWithdraw(amountAfterTakenFee, AffiliateProgramType.Bc7DonateFee),
                PaymentSystemType.BitBayPay,
                WithdrawalForHelper.ProjectDonationViaAffiliateProgramBc7DonateFee);

            BackgroundJob.Enqueue<CommitWithdrawalJob>(
                job => job.Execute(bc7DonateFeeWithdrawal, null));
            

            // TODO: WithdrawJob in the future to process automatically withdrawal via payment system to the user's btc wallet

            context.WriteLine("ProjectDonatedViaAffiliateProgramJob completed");
        }
    }
}