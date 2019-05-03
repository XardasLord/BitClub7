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
    public class UpgradeMatrixCommandHandler : IRequestHandler<UpgradeMatrixCommand, Guid> // TODO: Probably better would be returning object with status and Guid
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

        public async Task<Guid> Handle(UpgradeMatrixCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            _command = command;
            _lowerLevelMatrix = command.MatrixLevel - 1;
            _multiAccount = await _userMultiAccountRepository.GetAsync(command.UserMultiAccountId);

            await PreValidation();

            if (_multiAccount.UserAccountData.Role == UserRolesHelper.Admin && _userMatrixPositionOnLowerMatrix.DepthLevel == 2)
            {
                // This is the highest admin in the structure
                // TODO: Upgrade logic for admin

            }
            else
            {
                var adminPositionOnLowerMatrix = await _matrixPositionHelper.FindHighestAdminPositionAsync(_multiAccount.Id, _lowerLevelMatrix);
                if (adminPositionOnLowerMatrix?.UserMultiAccountId is null)
                {
                    throw new ValidationException($"FATAL! Admin does not exist in the structure in level: {_lowerLevelMatrix}");
                }

                // Algorytm szuka PIERWSZEGO ADMINa z góry i tylko on jest dla algorytmu wyznacznikiem.
                var adminAccountId = adminPositionOnLowerMatrix.UserMultiAccountId.Value;
                var adminPositionOnUpgradingLevel = await _matrixPositionRepository.GetPositionForAccountAtLevelAsync(adminAccountId, command.MatrixLevel);
                if (adminPositionOnUpgradingLevel is null)
                {
                    //TODO: Admin does not exist in the upgraded matrix level yet. Event and send e-mail to admin and send information back to requested user
                    return await Task.FromResult(Guid.Empty);
                }

                // TODO: Upgrade logic for user based on the admin

            }

            throw new NotImplementedException();
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
    }
}
