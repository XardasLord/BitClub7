using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Database;
using BC7.Infrastructure.CustomExceptions;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix
{
    public class BuyPositionInMatrixCommandHandler : IRequestHandler<BuyPositionInMatrixCommand, Guid>
    {
        private readonly IBitClub7Context _context;
        private readonly IUserMultiAccountHelper _userMultiAccountHelper;
        private readonly IMatrixPositionHelper _matrixPositionHelper;

        public BuyPositionInMatrixCommandHandler(
            IBitClub7Context context,
            IUserMultiAccountHelper userMultiAccountHelper,
            IMatrixPositionHelper matrixPositionHelper)
        {
            _context = context;
            _userMultiAccountHelper = userMultiAccountHelper;
            _matrixPositionHelper = matrixPositionHelper;
        }

        public async Task<Guid> Handle(BuyPositionInMatrixCommand request, CancellationToken cancellationToken)
        {
            var userMultiAccount = await _userMultiAccountHelper.GetById(request.UserMultiAccountId);

            // TODO: Validation parts:
            // 1. Check if user paid for the matrix position on this level (available in etap 1)

            // ReSharper disable once PossibleInvalidOperationException
            var invitingUserMatrix = await _matrixPositionHelper.GetMatrix(userMultiAccount.UserMultiAccountInvitingId.Value);
            if (!CheckIfMatrixHasEmptySpace(invitingUserMatrix))
            {
                throw new ValidationException("Matrix is full for the user from reflink");
            }


            throw new NotImplementedException();
        }

        private bool CheckIfMatrixHasEmptySpace(System.Collections.Generic.IEnumerable<Entity.MatrixPosition> invitingUserMatrix)
        {
            return _matrixPositionHelper.CheckIfMatrixHasEmptySpace(invitingUserMatrix);
        }
    }
}
