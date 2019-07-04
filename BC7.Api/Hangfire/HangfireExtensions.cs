using Hangfire;
using MediatR;
using Newtonsoft.Json;

namespace BC7.Api.Hangfire
{
    public static class HangfireExtensions
    {
        public static IGlobalConfiguration UseMediatR(this IGlobalConfiguration config, IMediator mediator)
        {
            config.UseActivator(new MediatRJobActivator(mediator));

            config.UseSerializerSettings(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

            return config;
        }
    }
}
