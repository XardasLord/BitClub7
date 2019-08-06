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
	public class DonationForFoundationJob : IJob<DonationForFoundationModel>
	{
		private readonly IWithdrawalHelper _withdrawalHelper;

		public DonationForFoundationJob(IWithdrawalHelper withdrawalHelper)
		{
			_withdrawalHelper = withdrawalHelper;
		}

		public Task Execute(DonationForFoundationModel donationForFoundationModel, PerformContext context)
		{
			context.WriteLine($"DonationForFoundationJob started with donationForFoundationModel data - {JsonConvert.SerializeObject(donationForFoundationModel)}.");

			var amountToWithdraw = _withdrawalHelper.CalculateAmountToWithdraw(donationForFoundationModel.Amount);

			var withdrawal = new Withdrawal(
				Guid.NewGuid(),
				donationForFoundationModel.DonatedUserMultiAccountId,
				amountToWithdraw,
				PaymentSystemType.BitBayPay,
				WithdrawalForHelper.DonationForFoundation);

			BackgroundJob.Enqueue<CommitWithdrawalJob>(
				job => job.Execute(withdrawal, null));

			// TODO: WithdrawJob in the future to process automatically withdrawal via payment system to the user's btc wallet

			context.WriteLine("DonationForFoundationJob completed.");

			return Task.CompletedTask;
		}
	}
}