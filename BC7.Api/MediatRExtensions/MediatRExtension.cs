using Hangfire;
using MediatR;

namespace BC7.Api.MediatRExtensions
{
    public static class MediatRExtension
    {
        public static void Enqueue(this IMediator mediator, IRequest request)
        {
            BackgroundJob.Enqueue<HangfireMediator>(m => m.SendCommand(request));
        }

        public static void Enqueue(this IMediator mediator, INotification @event)
        {
            BackgroundJob.Enqueue<HangfireMediator>(m => m.PublishEvent(@event));
        }
    }

    public class HangfireMediator
    {
        private readonly IMediator _mediator;

        public HangfireMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void SendCommand(IRequest request)
        {
            _mediator.Send(request);
        }

        public void PublishEvent(INotification @event)
        {
            _mediator.Publish(@event);
        }
    }
}
