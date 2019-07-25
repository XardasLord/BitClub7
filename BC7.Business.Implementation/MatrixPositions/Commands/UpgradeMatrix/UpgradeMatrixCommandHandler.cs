using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Business.Implementation.Jobs;
using BC7.Business.Implementation.Withdrawals.Jobs;
using BC7.Business.Implementation.Withdrawals.Jobs.JobModels;
using BC7.Business.Models;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using BC7.Security;
using Hangfire;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix
{
    public class UpgradeMatrixCommandHandler : IRequestHandler<UpgradeMatrixCommand, UpgradeMatrixResult>
    {
        private UpgradeMatrixCommand _command;
        private int _lowerLevelMatrix;
        private UserMultiAccount _multiAccount;
        private MatrixPosition _userMatrixPositionOnLowerMatrix;

        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IMatrixPositionRepository _matrixPositionRepository;
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IPaymentHistoryHelper _paymentHistoryHelper;
        private readonly IMatrixPositionHelper _matrixPositionHelper;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public UpgradeMatrixCommandHandler(
            IUserMultiAccountRepository userMultiAccountRepository,
            IMatrixPositionRepository matrixPositionRepository,
            IUserAccountDataRepository userAccountDataRepository,
            IPaymentHistoryHelper paymentHistoryHelper,
            IMatrixPositionHelper matrixPositionHelper,
            IBackgroundJobClient backgroundJobClient)
        {
            _userMultiAccountRepository = userMultiAccountRepository;
            _matrixPositionRepository = matrixPositionRepository;
            _userAccountDataRepository = userAccountDataRepository;
            _paymentHistoryHelper = paymentHistoryHelper;
            _matrixPositionHelper = matrixPositionHelper;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<UpgradeMatrixResult> Handle(UpgradeMatrixCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            _command = command;
            _lowerLevelMatrix = command.MatrixLevel - 1;
            _multiAccount = await _userMultiAccountRepository.GetAsync(command.UserMultiAccountId);

            await PreValidation();

            if (_multiAccount.UserAccountData.Role == UserRolesHelper.Admin && _userMatrixPositionOnLowerMatrix.DepthLevel == 2)
            {
                var upgradedPositionId = await UpgradeMatrixForAdmin();
                return new UpgradeMatrixResult(upgradedPositionId);
            }

            var adminPositionOnLowerMatrix = await _matrixPositionHelper.FindHighestAdminPositionAsync(_multiAccount.Id, _lowerLevelMatrix);
            if (adminPositionOnLowerMatrix?.UserMultiAccountId is null)
            {
                throw new ValidationException($"FATAL! Admin does not exist in the structure in level: {_lowerLevelMatrix}");
            }
                
            var adminAccountId = adminPositionOnLowerMatrix.UserMultiAccountId.Value;
            var adminPositionOnUpgradingLevel = await _matrixPositionHelper.GetPositionForAccountAtLevelAsync(adminAccountId, command.MatrixLevel);
            if (adminPositionOnUpgradingLevel is null)
            {
                //TODO: Event to notify admin that someone wants to upgrade his matrix
                //TODO: Use hangfire for this
                return new UpgradeMatrixResult(Guid.Empty, "Cannot upgrade because admin didn't upgrade his matrix yet. E-mail notification has been sent to admin");
            }

            var adminSide = await _matrixPositionHelper.GetAdminStructureSide(_multiAccount.Id, _lowerLevelMatrix, adminPositionOnLowerMatrix);
            var upgradedMatrixPositionId = await UpgradeMatrixForUser(adminPositionOnUpgradingLevel, adminSide);

            return new UpgradeMatrixResult(upgradedMatrixPositionId);
        }

        private async Task PreValidation()
        {
            if (_multiAccount is null)
            {
                throw new ValidationException("User with given ID was not found");
            }

            _userMatrixPositionOnLowerMatrix = await _matrixPositionHelper.GetPositionForAccountAtLevelAsync(_multiAccount.Id, _lowerLevelMatrix);
            if (_userMatrixPositionOnLowerMatrix is null)
            {
                throw new ValidationException($"User does not have position in the lower level matrix: {_lowerLevelMatrix}");
            }

            if (await _paymentHistoryHelper.DoesUserPaidForMatrixLevelAsync(_command.MatrixLevel, _multiAccount.Id) == false)
            {
                throw new ValidationException("User didn't pay for the upgraded matrix yet");
            }
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

        private async Task<Guid> UpgradeMatrixForUser(MatrixPosition adminPosition, AdminStructureSide adminStructure)
        {
            MatrixPosition upgradedPosition;

            if (!_multiAccount.SponsorId.HasValue)
            {
                throw new ValidationException("FATAL! User does not have sponsor");
            }

            var userAccount = await _userAccountDataRepository.GetAsync(_multiAccount.UserAccountDataId);
            var userMultiAccountIds = userAccount.UserMultiAccounts.Select(x => x.Id).ToList(); // Need for cycles in the future

            var sponsorPositionOnUpgradedMatrix = await _matrixPositionHelper.GetPositionForAccountAtLevelAsync(_multiAccount.SponsorId.Value, _command.MatrixLevel);
            if (sponsorPositionOnUpgradedMatrix is null)
            {
                upgradedPosition = await _matrixPositionHelper.FindTheNearestEmptyPositionFromGivenAccountWhereInParentsMatrixThereIsNoAnyMultiAccountAsync(
                    adminPosition.UserMultiAccountId.Value, userMultiAccountIds, _command.MatrixLevel, adminStructure);
            }
            else
            {
                upgradedPosition = await _matrixPositionHelper.FindTheNearestEmptyPositionFromGivenAccountWhereInParentsMatrixThereIsNoAnyMultiAccountAsync(
                    _multiAccount.SponsorId.Value, userMultiAccountIds, _command.MatrixLevel);
            }

            if (upgradedPosition is null)
            {
                throw new ValidationException($"There is no empty space in matrix level - {_command.MatrixLevel} - where account can be assigned");
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
