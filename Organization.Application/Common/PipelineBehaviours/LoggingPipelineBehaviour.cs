using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Policy;

namespace Organization.Application.Common.PipelineBehaviours
{
    public sealed class LoggingPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        private readonly ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> _logger;
        public LoggingPipelineBehaviour(ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{RequestName} has started at {DateTime}", typeof(TRequest).Name, DateTime.UtcNow);
            var timer = Stopwatch.StartNew();
            var result = await next();
            timer.Stop();
            if(result.IsError)
            {
                _logger.LogInformation("{RequestName} has failed at {DateTime}", typeof(TRequest).Name, DateTime.UtcNow);
            }
            _logger.LogInformation("{RequestName} has completed successfully in {Milliseconds}ms.", typeof(TRequest).Name, timer.ElapsedMilliseconds);
            return result;
        }
    }
}
