using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Database;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix
{
    public class BuyPositionInMatrixCommandHandler : IRequestHandler<BuyPositionInMatrixCommand, Guid>
    {
        private readonly IBitClub7Context _context;

        public BuyPositionInMatrixCommandHandler(IBitClub7Context context)
        {
            _context = context;
        }

        public Task<Guid> Handle(BuyPositionInMatrixCommand request, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
        }
    }
}
