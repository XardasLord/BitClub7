using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix
{
    public class UpgradeMatrixCommandHandler : IRequestHandler<UpgradeMatrixCommand, Guid>
    {
        private readonly IUserMultiAccountRepository _userMultiAccountRepository;
        private readonly IMatrixPositionRepository _matrixPositionRepository;
        private readonly IPaymentHistoryHelper _paymentHistoryHelper;

        public UpgradeMatrixCommandHandler(
            IUserMultiAccountRepository userMultiAccountRepository,
            IMatrixPositionRepository matrixPositionRepository,
            IPaymentHistoryHelper paymentHistoryHelper)
        {
            _userMultiAccountRepository = userMultiAccountRepository;
            _matrixPositionRepository = matrixPositionRepository;
            _paymentHistoryHelper = paymentHistoryHelper;
        }

        public async Task<Guid> Handle(UpgradeMatrixCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var multiAccount = await _userMultiAccountRepository.GetAsync(command.UserMultiAccountId);
            if (multiAccount is null)
            {
                throw new ValidationException("User with given ID was not found");
            }
            
            var lowerLevelMatrix = command.MatrixLevel - 1;
            if (await _matrixPositionRepository.GetMatrixForGivenMultiAccountAsync(multiAccount.Id, lowerLevelMatrix) is null)
            {
                throw new ValidationException($"User does not have position in the lower level matrix: {lowerLevelMatrix}");
            }
            
            if (await _paymentHistoryHelper.DoesUserPaidForMatrixLevelAsync(command.MatrixLevel, multiAccount.Id) == false)
            {
                throw new ValidationException("User didn't pay for the upgraded matrix yet");
            }

            // 4. Sprawdzenie czy admin (znajdujący się na samej górze matrycy o level niższej) ma wykupione już miejsce w matrycy, do której chcemy zrobić upgrade


            // TODO: REST LOGIC

            throw new NotImplementedException();
        }
    }
}
