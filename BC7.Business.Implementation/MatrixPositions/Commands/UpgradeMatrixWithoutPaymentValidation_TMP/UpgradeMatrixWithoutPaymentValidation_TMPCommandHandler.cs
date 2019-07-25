using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Business.Implementation.Jobs;
using BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix;
using BC7.Business.Implementation.Withdrawals.Jobs;
using BC7.Business.Implementation.Withdrawals.Jobs.JobModels;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using BC7.Security;
using Hangfire;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrixWithoutPaymentValidation_TMP
{
    public class UpgradeMatrixWithoutPaymentValidation_TMPCommandHandler : IRequestHandler<UpgradeMatrixWithoutPaymentValidation_TMPCommand, UpgradeMatrixResult>
    {
        private UpgradeMatrixWithoutPaymentValidation_TMPCommand _command;
        private int _lowerLevelMatrix;
        private UserMultiAccount _multiAccount;

        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IMatrixPositionRepository _matrixPositionRepository;
        private readonly IMatrixPositionHelper _matrixPositionHelper;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public UpgradeMatrixWithoutPaymentValidation_TMPCommandHandler(
            IUserMultiAccountRepository userMultiAccountRepository,
            IMatrixPositionRepository matrixPositionRepository,
            IMatrixPositionHelper matrixPositionHelper,
            IBackgroundJobClient backgroundJobClient)
        {
            _userMultiAccountRepository = userMultiAccountRepository;
            _matrixPositionRepository = matrixPositionRepository;
            _matrixPositionHelper = matrixPositionHelper;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<UpgradeMatrixResult> Handle(UpgradeMatrixWithoutPaymentValidation_TMPCommand command, CancellationToken cancellationToken)
        {
            _command = command;
            _lowerLevelMatrix = command.MatrixLevel - 1;
            _multiAccount = await _userMultiAccountRepository.GetAsync(command.UserMultiAccountId);

            var _userMatrixPositionOnLowerMatrix = await _matrixPositionHelper.GetPositionForAccountAtLevelAsync(_multiAccount.Id, _lowerLevelMatrix);

            if (_multiAccount.UserAccountData.Role == UserRolesHelper.Admin && _userMatrixPositionOnLowerMatrix.DepthLevel == 2)
            {
                var upgradedPositionId = await UpgradeMatrixForAdmin();
                return new UpgradeMatrixResult(upgradedPositionId);
            }

            return new UpgradeMatrixResult(Guid.Empty, "Cannot upgrade UpgradeMatrixWithoutPaymentValidation_TMPCommandHandler.");
        }

        private async Task<Guid> UpgradeMatrixForAdmin()
        {
            var upgradedPosition = await _matrixPositionHelper.FindEmptyPositionForHighestAdminAsync(_command.MatrixLevel);
            if (upgradedPosition is null)
            {
                throw new ValidationException($"FATAL! There is no empty space for Admin to upgrade matrix at level: {_command.MatrixLevel}");
            }

            upgradedPosition.AssignMultiAccount(_multiAccount.Id);

            await _matrixPositionRepository.UpdateAsync(upgradedPosition);

            _backgroundJobClient.Enqueue<MatrixPositionHasBeenUpgradedJob>(
                job => job.Execute(upgradedPosition.Id, null));

            _backgroundJobClient.Enqueue<InitWithdrawalJob>(
                job => job.Execute(new InitWithdrawalModel
                {
                    MatrixPositionId = upgradedPosition.Id,
                    WithdrawalFor = WithdrawalForHelper.UpgradedMatrix
                }, null));

            return upgradedPosition.Id;
        }
    }
}