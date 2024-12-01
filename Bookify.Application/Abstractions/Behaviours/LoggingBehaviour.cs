using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Abstractions.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IBaseRequest
      where TResponse: Result
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;
            try {
                _logger.LogInformation("Executing request {Request}", name);
                var result = await next();
                if (result.IsSuccess)
                {
                    _logger.LogInformation("Request {Request} processed successfully", name);
                }
                else
                {
                    using (LogContext.PushProperty("Error", result.Error, true)) {
                        _logger.LogError(
                            "Request {Request} processed with {Error}",
                            name,
                            result.Error);
                    }
                }
                
                return result; 
            } catch(Exception exception)
            {
                _logger.LogError(exception, "Request {Request} processing failed", name);
                throw; 
            }
        }
    }
}
