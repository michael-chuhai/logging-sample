using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MySecrets.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(
            RequestDelegate next,
            ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IOptions<MySecretsOptions> options)
        {
            var stopwatch = new Stopwatch();
            var requestUrl = context.Request.GetDisplayUrl();

            using (_logger.BeginScope("Request ID: {requestId}", Guid.NewGuid()))
            {
                stopwatch.Start();

                try
                {
                    await _next(context);

                    stopwatch.Stop();

                    if (stopwatch.Elapsed > options.Value.AcceptableExecutionTime)
                    {
                        _logger.LogWarning("The request handling took too much time. Request URL: {requestUrl}", requestUrl);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to execute the request: {url}. And also some custom data could be logged here.", requestUrl);

                    throw;
                }
            }
        }
    }
}