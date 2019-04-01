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

        public Task<Guid> Handle(BuyPositionInMatrixCommand request, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
        }
    }
}
