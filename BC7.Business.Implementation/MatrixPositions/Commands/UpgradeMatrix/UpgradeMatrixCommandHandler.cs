using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix
{
    public class UpgradeMatrixCommandHandler : IRequestHandler<UpgradeMatrixCommand, Guid>
    {
        public UpgradeMatrixCommandHandler()
        {
        }

        public Task<Guid> Handle(UpgradeMatrixCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
