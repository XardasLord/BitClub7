using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BC7.Infrastructure.Implementation
{
    public class RequestPreProcessorLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;

        public RequestPreProcessorLogger(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var name = typeof(TRequest).Name;
            var requestJson = JsonConvert.SerializeObject(request);

            _logger.LogInformation($"Request: {name} {requestJson}");

            return Task.CompletedTask;
        }
    }
}
