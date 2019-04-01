using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Business.Helpers;
using BC7.Database;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix
{
    public class BuyPositionInMatrixCommandHandler : IRequestHandler<BuyPositionInMatrixCommand, Guid>
    {
        private readonly IBitClub7Context _context;
        private readonly IUserMultiAccountHelper _userMultiAccountHelper;

        public BuyPositionInMatrixCommandHandler(IBitClub7Context context, IUserMultiAccountHelper userMultiAccountHelper)
        {
            _context = context;
            _userMultiAccountHelper = userMultiAccountHelper;
        }

        public async Task<Guid> Handle(BuyPositionInMatrixCommand request, CancellationToken cancellationToken)
        {
            var userMultiAccount = await _userMultiAccountHelper.GetById(request.UserMultiAccountId);

            // TODO: Validation parts:
            // 1. Check if user paid for the matrix position on this level (available in etap 1)
            // 2. Check if user from reflink (UserMultiAccountInviting) has space in his matrix in this level. (shouldn't it be pre-validated when multi account is creating ???)
            // We can get UserMultiAccountInviting and take 2 levels below (should be 7 users)

            throw new NotImplementedException();
        }
    }
}
