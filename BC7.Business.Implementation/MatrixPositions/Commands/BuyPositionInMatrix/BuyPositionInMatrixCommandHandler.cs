using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Business.Implementation.Events;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Infrastructure.Implementation.Hangfire;
using BC7.Repository;
using MediatR;
// ReSharper disable PossibleInvalidOperationException
// ReSharper disable PossibleMultipleEnumeration

namespace BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix
{
    public class BuyPositionInMatrixCommandHandler : IRequestHandler<BuyPositionInMatrixCommand, Guid>
    {
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IMatrixPositionRepository _matrixPositionRepository;
        private readonly IMatrixPositionHelper _matrixPositionHelper;
        private readonly IPaymentHistoryHelper _paymentHistoryHelper;
        private readonly IMediator _mediator;

        public BuyPositionInMatrixCommandHandler(
            IUserMultiAccountRepository userMultiAccountRepository,
            IUserAccountDataRepository userAccountDataRepository,
            IMatrixPositionRepository matrixPositionRepository,
            IMatrixPositionHelper matrixPositionHelper,
            IPaymentHistoryHelper paymentHistoryHelper,
            IMediator mediator)
        {
            _userMultiAccountRepository = userMultiAccountRepository;
            _userAccountDataRepository = userAccountDataRepository;
            _matrixPositionRepository = matrixPositionRepository;
            _matrixPositionHelper = matrixPositionHelper;
            _paymentHistoryHelper = paymentHistoryHelper;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(BuyPositionInMatrixCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userMultiAccount = await _userMultiAccountRepository.GetAsync(command.UserMultiAccountId);

            await ValidateUserMultiAccount(userMultiAccount, command.MatrixLevel);

            var sponsorAccountId = userMultiAccount.SponsorId.Value;

            var invitingUserMatrix = await _matrixPositionHelper.GetMatrixForGivenMultiAccountAsync(sponsorAccountId, command.MatrixLevel);
            if (invitingUserMatrix is null)
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
                var userAccount = await _userAccountDataRepository.GetAsync(userMultiAccount.UserAccountDataId);
                var userMultiAccountIds = userAccount.UserMultiAccounts.Select(x => x.Id).ToList();

                matrixPosition = await _matrixPositionHelper.FindTheNearestEmptyPositionFromGivenAccountWhereInParentsMatrixThereIsNoAnyMultiAccountAsync(
                            sponsorAccountId, userMultiAccountIds, command.MatrixLevel);

                if (matrixPosition is null)
                {
                    throw new ValidationException("There is no empty space in matrix where account can be assigned");
                }

                await ChangeUserSponsor(userMultiAccount, matrixPosition);
            }

            matrixPosition.AssignMultiAccount(command.UserMultiAccountId);
            await _matrixPositionRepository.UpdateAsync(matrixPosition);

            PublishMatrixPositionHasBeenBoughtNotification(matrixPosition.Id);
            PublishUserBoughtMatrixPositionNotification(userMultiAccount.Id);

            return matrixPosition.Id;
        }


        private async Task ValidateUserMultiAccount(UserMultiAccount userMultiAccount, int matrixLevel)
        {
            if (userMultiAccount is null) throw new ArgumentNullException(nameof(userMultiAccount));
            if (userMultiAccount.MatrixPositions.Any())
            {
                throw new ValidationException("This account already exists in a matrix");
            }

            if (userMultiAccount.UserAccountData.IsMembershipFeePaid == false)
            {
                throw new ValidationException("The main account did not buy pay the membership's fee yet");
            }

            if (await _paymentHistoryHelper.DoesUserPaidForMatrixLevelAsync(matrixLevel, userMultiAccount.Id) == false)
            {
                throw new ValidationException($"User didn't pay for the matrix at level {matrixLevel} yet");
            }

            if (!userMultiAccount.SponsorId.HasValue)
            {
                throw new ValidationException("This account does not have inviting multi account set");
            }
        }

        private async Task ChangeUserSponsor(UserMultiAccount userMultiAccount, MatrixPosition matrixPosition)
        {
            var parentPosition = await _matrixPositionRepository.GetAsync(matrixPosition.ParentId.Value);

            userMultiAccount.ChangeSponsor(parentPosition.UserMultiAccountId.Value);
            await _userMultiAccountRepository.UpdateAsync(userMultiAccount);
        }

        private void PublishMatrixPositionHasBeenBoughtNotification(Guid matrixPositionId)
        {
            var @event = new MatrixPositionHasBeenBoughtEvent(matrixPositionId);
            _mediator.Enqueue(@event);
        }

        private void PublishUserBoughtMatrixPositionNotification(Guid userMultiAccountId)
        {
            var @event = new UserBoughtMatrixPositionEvent { MultiAccountId = userMultiAccountId };
            _mediator.Enqueue(@event);
        }
    }
}
