using System;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Domain;
using BC7.Domain.Enums;
using BC7.Infrastructure.Implementation.Hangfire;
using BC7.Repository;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Newtonsoft.Json;

namespace BC7.Business.Implementation.Withdrawals.Jobs
{
    public class InitWithdrawalJob : IJob<Guid>
    {
        private readonly IMatrixPositionRepository _matrixPositionRepository;
        private readonly IMatrixPositionHelper _matrixPositionHelper;
        private readonly IWithdrawalHelper _withdrawalHelper;

        public InitWithdrawalJob(IMatrixPositionRepository matrixPositionRepository, IMatrixPositionHelper matrixPositionHelper, IWithdrawalHelper withdrawalHelper)
        {
            _matrixPositionRepository = matrixPositionRepository;
            _matrixPositionHelper = matrixPositionHelper;
            _withdrawalHelper = withdrawalHelper;
        }

        public async Task Execute(Guid boughtMatrixPositionId, PerformContext context)
        {
            context.WriteLine($"InitWithdrawalJob started with boughtMatrixPositionId - {boughtMatrixPositionId}");

            var matrixPositionInLineB = await _matrixPositionRepository.GetAsync(boughtMatrixPositionId);

            context.WriteLine($"Bought matrix position data: {JsonConvert.SerializeObject(matrixPositionInLineB)}");

            var matrixOwner = await _matrixPositionHelper.GetTopParentAsync(matrixPositionInLineB, matrixPositionInLineB.MatrixLevel);

            context.WriteLine($"Matrix owner data: {JsonConvert.SerializeObject(matrixOwner)}");

            if (!matrixOwner.UserMultiAccountId.HasValue)
            {
                throw new InvalidOperationException($"Matrix owner position with ID {matrixOwner.Id} does not assigned multi account.");
            }

            var amountToWithdraw = _withdrawalHelper.CalculateAmountToWithdraw(matrixOwner.MatrixLevel);

            var withdrawal = new Withdrawal(
                Guid.NewGuid(),
                matrixOwner.UserMultiAccountId.Value,
                amountToWithdraw,
                PaymentSystemType.BitBayPay,
                WithdrawalForHelper.AssignmentInMatrix);

            BackgroundJob.Enqueue<CommitWithdrawalJob>(
                job => job.Execute(withdrawal, null));

            // TODO: WithdrawJob in the future to process automatically withdrawal via payment system to the user's btc wallet

            context.WriteLine("InitWithdrawalJob completed");
        }
    }
}
