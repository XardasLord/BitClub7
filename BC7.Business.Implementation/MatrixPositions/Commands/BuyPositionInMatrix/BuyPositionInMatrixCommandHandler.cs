using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Database;
using BC7.Entity;
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

            if (userMultiAccount.MatrixPositions.Any())
            {
                throw new ValidationException("This account already exists in a matrix");
            }

            // 1. Check if user paid for the matrix position on this level (available in etap 1)

            // ReSharper disable once PossibleInvalidOperationException
            var invitingUserMatrix = await _matrixPositionHelper.GetMatrix(userMultiAccount.UserMultiAccountInvitingId.Value);
            if (!CheckIfMatrixHasEmptySpace(invitingUserMatrix))
            {
                // Maybe we should find another sponsor instead of throwing an error here?
                throw new ValidationException("Matrix is full for the user from reflink");
            }

            // Buying position in matrix

            throw new NotImplementedException();
        }

        private bool CheckIfMatrixHasEmptySpace(System.Collections.Generic.IEnumerable<Entity.MatrixPosition> invitingUserMatrix)
        {
            return _matrixPositionHelper.CheckIfMatrixHasEmptySpace(invitingUserMatrix);
        }
    }
}
