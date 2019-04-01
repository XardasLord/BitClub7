using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix
{
    public class BuyPositionInMatrixCommandHandler : IRequestHandler<BuyPositionInMatrixCommand, Guid>
    {
        public Task<Guid> Handle(BuyPositionInMatrixCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
