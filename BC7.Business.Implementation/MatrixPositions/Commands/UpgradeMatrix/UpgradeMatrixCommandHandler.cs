using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using BC7.Security;
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
        private readonly IPaymentHistoryHelper _paymentHistoryHelper;
        private readonly IMatrixPositionHelper _matrixPositionHelper;

        public UpgradeMatrixCommandHandler(
            IUserMultiAccountRepository userMultiAccountRepository,
            IMatrixPositionRepository matrixPositionRepository,
            IPaymentHistoryHelper paymentHistoryHelper,
            IMatrixPositionHelper matrixPositionHelper)
        {
            _userMultiAccountRepository = userMultiAccountRepository;
            _matrixPositionRepository = matrixPositionRepository;
            _paymentHistoryHelper = paymentHistoryHelper;
            _matrixPositionHelper = matrixPositionHelper;
        }

        public async Task<UpgradeMatrixResult> Handle(UpgradeMatrixCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            _command = command;
            _lowerLevelMatrix = command.MatrixLevel - 1;
            _multiAccount = await _userMultiAccountRepository.GetAsync(command.UserMultiAccountId);

            await PreValidation();

            if (_multiAccount.UserAccountData.Role == UserRolesHelper.Admin && _userMatrixPositionOnLowerMatrix.DepthLevel == 2)
            {
                // Upgrade for admin
                var upgradedPositionId = await UpgradeMatrixForAdmin();
                return new UpgradeMatrixResult(upgradedPositionId);
            }

            var adminPositionOnLowerMatrix = await _matrixPositionHelper.FindHighestAdminPositionAsync(_multiAccount.Id, _lowerLevelMatrix);
            if (adminPositionOnLowerMatrix?.UserMultiAccountId is null)
            {
                throw new ValidationException($"FATAL! Admin does not exist in the structure in level: {_lowerLevelMatrix}");
            }
                
            var adminAccountId = adminPositionOnLowerMatrix.UserMultiAccountId.Value;
            var adminPositionOnUpgradingLevel = await _matrixPositionRepository.GetPositionForAccountAtLevelAsync(adminAccountId, command.MatrixLevel);
            if (adminPositionOnUpgradingLevel is null)
            {
                //TODO: Event to notify admin that someone wants to upgrade his matrix
                return new UpgradeMatrixResult(Guid.Empty, "Cannot upgrade because admin didn't upgrade his matrix yet. E-mail notification has been sent to admin");
            }

            // Upgrade for normal user
            var upgradedMatrixPositionId = await UpgradeMatrixForUser(adminPositionOnUpgradingLevel);
            return new UpgradeMatrixResult(upgradedMatrixPositionId);
        }

        private async Task PreValidation()
        {
            if (_multiAccount is null)
            {
                throw new ValidationException("User with given ID was not found");
            }

            _userMatrixPositionOnLowerMatrix = await _matrixPositionRepository.GetPositionForAccountAtLevelAsync(_multiAccount.Id, _lowerLevelMatrix);
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

            return upgradedPosition.Id;
        }

        private Task<Guid> UpgradeMatrixForUser(MatrixPosition adminPosition)
        {
            // TODO: Upgrade logic for user based on the admin - to rethink how to do it clearly

            throw new NotImplementedException();
        }
    }
}
