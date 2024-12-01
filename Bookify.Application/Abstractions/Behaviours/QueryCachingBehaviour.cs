using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Abstractions.Behaviors
{
    internal sealed class QueryCachingBehaviour<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachedQuery
        where TResponse : Result
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<QueryCachingBehaviour<TRequest, TResponse>> _logger;
        public QueryCachingBehaviour(ICacheService cacheService, ILogger<QueryCachingBehaviour<TRequest, TResponse>> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

      
        async Task<TResponse> IPipelineBehavior<TRequest, TResponse>.Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse? cachedResult = await _cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);
            string name = typeof(TRequest).Name;
            if(cachedResult is not null)
            {
                _logger.LogInformation("Cache hit for {Query}", name);
                return cachedResult;
            }
            _logger.LogInformation("Cache miss for {Query}", name);
            var result = await next();
            if (result.IsSuccess) { 
              await _cacheService.SetAsync(request.CacheKey, result,request.Expiration, cancellationToken);
            }
            return result;
        }
    }
}
