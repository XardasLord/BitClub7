using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BC7.Business.Implementation.Payments.Events
{
    public class PaymentStatusChangedEventHandler :INotificationHandler<PaymentStatusChangedEvent>
    {
        public Task Handle(PaymentStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
