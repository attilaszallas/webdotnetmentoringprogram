using Microsoft.AspNetCore.Mvc.Filters;

namespace WebDotNetMentoringProgram.Filters
{
    public class LoggingResponseHeaderFilterService : IResultFilter
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private bool _isLoggingFilterEnabled;


        public LoggingResponseHeaderFilterService(
                ILogger<LoggingResponseHeaderFilterService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;

            _isLoggingFilterEnabled = _configuration["IsLoggingFilterEnabledFor"].Contains(actionName);

            if (_isLoggingFilterEnabled)
            {
                _logger.LogInformation(
                    $"- {nameof(LoggingResponseHeaderFilterService)}.{nameof(OnResultExecuting)}");
                /*
                context.HttpContext.Response.Headers.Add(
                    nameof(OnResultExecuting), nameof(LoggingResponseHeaderFilterService));
                */
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (_isLoggingFilterEnabled)
            {
                _logger.LogInformation(
                    $"- {nameof(LoggingResponseHeaderFilterService)}.{nameof(OnResultExecuted)}");
            }
        }
    }
}
