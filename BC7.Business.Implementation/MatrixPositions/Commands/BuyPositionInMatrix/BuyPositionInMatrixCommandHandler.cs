using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Business.Implementation.Events;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using MediatR;
// ReSharper disable PossibleInvalidOperationException
// ReSharper disable PossibleMultipleEnumeration

namespace BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix
{
    public class BuyPositionInMatrixCommandHandler : IRequestHandler<BuyPositionInMatrixCommand, Guid>
    {
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IMatrixPositionRepository _matrixPositionRepository;
        private readonly IMatrixPositionHelper _matrixPositionHelper;
        private readonly IMediator _mediator;

        public BuyPositionInMatrixCommandHandler(
            IUserMultiAccountRepository userMultiAccountRepository,
            IMatrixPositionRepository matrixPositionRepository,
            IMatrixPositionHelper matrixPositionHelper,
            IMediator mediator)
        {
            _userMultiAccountRepository = userMultiAccountRepository;
            _matrixPositionRepository = matrixPositionRepository;
            _matrixPositionHelper = matrixPositionHelper;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(BuyPositionInMatrixCommand command, CancellationToken cancellationToken)
        {
            var userMultiAccount = await _userMultiAccountRepository.GetAsync(command.UserMultiAccountId);

            ValidateUserMultiAccount(userMultiAccount);

            var sponsorAccountId = userMultiAccount.UserMultiAccountInvitingId.Value;

            var invitingUserMatrix = await _matrixPositionRepository.GetMatrixAsync(sponsorAccountId, command.MatrixLevel);
            if (invitingUserMatrix == null)
            {
                throw new ValidationException($"The inviting user from reflink does not have matrix on level: {command.MatrixLevel}");
            }

            MatrixPosition matrixPosition;
            if (_matrixPositionHelper.CheckIfMatrixHasEmptySpace(invitingUserMatrix))
            {
                matrixPosition = invitingUserMatrix.First(x => x.UserMultiAccountId == null);
            }
            else
            {
                matrixPosition = await _matrixPositionHelper.FindTheNearestEmptyPositionFromGivenAccount(sponsorAccountId, command.MatrixLevel);
                // TODO: We should probably validate again to check if this founded position is not in the any of another owner multiAccounts (command.UserMultiAccountId other accounts)

                // TODO: Should we also change the sponsor for the founder of founded matrix?
                // If yes than helper should has a method called like `GetSponsorForGivenPosition()`
            }

            matrixPosition.AssignMultiAccount(command.UserMultiAccountId);
            await _matrixPositionRepository.UpdateAsync(matrixPosition);

            await PublishMatrixPositionHasBeenBoughtNotification(matrixPosition.Id);
            await PublishUserBoughtMatrixPositionNotification(userMultiAccount.Id);

            return matrixPosition.Id;
        }

        private static void ValidateUserMultiAccount(UserMultiAccount userMultiAccount)
        {
            if (userMultiAccount == null) throw new ArgumentNullException(nameof(userMultiAccount));
            if (userMultiAccount.MatrixPositions.Any())
            {
                throw new ValidationException("This account already exists in a matrix");
            }

            // TODO: Add check if user paid for the matrix position on this level (available in etap 1)

            if (!userMultiAccount.UserMultiAccountInvitingId.HasValue)
            {
                throw new ValidationException("This account does not have inviting multi account set");
            }
        }

        private async Task PublishMatrixPositionHasBeenBoughtNotification(Guid matrixPositionId)
        {
            var @event = new MatrixPositionHasBeenBoughtEvent { MatrixPositionId = matrixPositionId };
            await _mediator.Publish(@event);
        }

        private async Task PublishUserBoughtMatrixPositionNotification(Guid userMultiAccountId)
        {
            var @event = new UserBoughtMatrixPositionEvent { MultiAccountId = userMultiAccountId };
            await _mediator.Publish(@event);
        }
    }
}
