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
    class ProjectDonatedJob : IJob<ProjectDonatedModel>
    {
        private readonly IWithdrawalHelper _withdrawalHelper;

        public ProjectDonatedJob(IWithdrawalHelper withdrawalHelper)
        {
            _withdrawalHelper = withdrawalHelper;
        }

        public Task Execute(ProjectDonatedModel projectDonatedModel, PerformContext context)
        {
            context.WriteLine($"ProjectDonatedJob started with projectDonatedModel data - {JsonConvert.SerializeObject(projectDonatedModel)}");

            var amountToWithdraw = _withdrawalHelper.CalculateAmountToWithdraw(projectDonatedModel.Amount);

            var withdrawal = new Withdrawal(
                Guid.NewGuid(),
                projectDonatedModel.DonatedUserMultiAccountId,
                amountToWithdraw,
                PaymentSystemType.BitBayPay,
                WithdrawalForHelper.ProjectDonation);

            BackgroundJob.Enqueue<CommitWithdrawalJob>(
                job => job.Execute(withdrawal, null));

            // TODO: WithdrawJob in the future to process automatically withdrawal via payment system to the user's btc wallet

            context.WriteLine("ProjectDonatedJob completed");

            return Task.CompletedTask;
        }
    }
}
