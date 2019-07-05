using System;
using BC7.Infrastructure.Implementation.Hangfire;
using Hangfire;
using MediatR;

namespace BC7.Api.Hangfire
{
    public class MediatRJobActivator : JobActivator
    {
        private readonly IMediator _mediator;

        public MediatRJobActivator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override object ActivateJob(Type type)
        {
            return new HangfireMediator(_mediator);
        }
    }
}
