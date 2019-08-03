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

            ScheduleWithdrawalJob(
                _rootId, 
                bc7Fee, 
                WithdrawalForHelper.ProjectDonationViaAffiliateProgramBc7ConstFee);
            
            var amountAfterTakenFee = model.Amount - bc7Fee;


            // DonatedDirectly (80%)
            ScheduleWithdrawalJob(
                model.DonatedUserMultiAccountId, 
                _withdrawalHelper.CalculateAmountToWithdraw(amountAfterTakenFee, AffiliateProgramType.DirectDonate),
                WithdrawalForHelper.ProjectDonationViaAffiliateProgramDirectDonate);


            // DonatedLineA (10%)
            var lineAMultiAccount = await _userMultiAccountHelper.GetSponsorForMultiAccount(model.DonatedUserMultiAccountId);

            ScheduleWithdrawalJob(
                lineAMultiAccount.Id,
                _withdrawalHelper.CalculateAmountToWithdraw(amountAfterTakenFee, AffiliateProgramType.AffiliateLineA),
                WithdrawalForHelper.ProjectDonationViaAffiliateProgramLineA);


            // DonatedLineB (5%)
            var lineBMultiAccount = await _userMultiAccountHelper.GetSponsorForMultiAccount(lineAMultiAccount.Id);

            ScheduleWithdrawalJob(
                lineBMultiAccount.Id,
                _withdrawalHelper.CalculateAmountToWithdraw(amountAfterTakenFee, AffiliateProgramType.AffiliateLineB),
                WithdrawalForHelper.ProjectDonationViaAffiliateProgramLineB);


            // BC7DonatedFee (5%)
            ScheduleWithdrawalJob(
                _rootId,
                _withdrawalHelper.CalculateAmountToWithdraw(amountAfterTakenFee, AffiliateProgramType.Bc7DonateFee),
                WithdrawalForHelper.ProjectDonationViaAffiliateProgramBc7DonateFee);
            

            // TODO: WithdrawJob in the future to process automatically withdrawal via payment system to the user's btc wallet

            context.WriteLine("ProjectDonatedViaAffiliateProgramJob completed");
        }

        private void ScheduleWithdrawalJob(Guid multiAccountId, decimal amount, string withdrawalDescription)
        {
            var withdrawal = new Withdrawal(
                Guid.NewGuid(),
                multiAccountId,
                amount,
                PaymentSystemType.BitBayPay,
                withdrawalDescription);

            BackgroundJob.Enqueue<CommitWithdrawalJob>(
                job => job.Execute(withdrawal, null));
        }
    }
}