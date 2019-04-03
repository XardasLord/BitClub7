using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BC7.Business.Implementation.Events
{
    public class MatrixPositionBoughtEventHandler : INotificationHandler<MatrixPositionBoughtEvent>
    {
        public Task Handle(MatrixPositionBoughtEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
