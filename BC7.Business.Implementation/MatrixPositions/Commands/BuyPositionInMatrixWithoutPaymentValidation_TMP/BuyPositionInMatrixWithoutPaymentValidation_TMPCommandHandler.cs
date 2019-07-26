using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Business.Implementation.Jobs;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using Hangfire;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrixWithoutPaymentValidation_TMP
{
    public class BuyPositionInMatrixWithoutPaymentValidation_TMPCommandHandler : IRequestHandler<BuyPositionInMatrixWithoutPaymentValidation_TMPCommand, Guid>
    {
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IMatrixPositionRepository _matrixPositionRepository;
        private readonly IMatrixPositionHelper _matrixPositionHelper;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public BuyPositionInMatrixWithoutPaymentValidation_TMPCommandHandler(
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

        public async Task<Guid> Handle(BuyPositionInMatrixWithoutPaymentValidation_TMPCommand command, CancellationToken cancellationToken)
        {
            var userMultiAccount = await _userMultiAccountRepository.GetAsync(command.UserMultiAccountId);
            var sponsorAccountId = userMultiAccount.SponsorId.Value;

            var invitingUserMatrix = await _matrixPositionHelper.GetMatrixForGivenMultiAccountAsync(sponsorAccountId, command.MatrixLevel);
            if (invitingUserMatrix is null)
            {
                throw new ValidationException($"The inviting user from reflink does not have matrix on level: {command.MatrixLevel}");
            }

            MatrixPosition matrixPosition;
            var matrixPositions = invitingUserMatrix as MatrixPosition[] ?? invitingUserMatrix.ToArray();
            if (_matrixPositionHelper.CheckIfMatrixHasEmptySpace(matrixPositions))
            {
                matrixPosition = matrixPositions
                    .OrderBy(x => x.DepthLevel)
                    .First(x => x.UserMultiAccountId == null);
            }
            else
            {
                throw new ValidationException("There is no empty space in matrix");
            }

            matrixPosition.AssignMultiAccount(command.UserMultiAccountId);
            await _matrixPositionRepository.UpdateAsync(matrixPosition);

            _backgroundJobClient.Enqueue<MatrixPositionHasBeenBoughtJob>(
                job => job.Execute(matrixPosition.Id, null));

            _backgroundJobClient.Enqueue<UserBoughtMatrixPositionJob>(
                job => job.Execute(userMultiAccount.Id, null));

            return matrixPosition.Id;
        }
    }
}