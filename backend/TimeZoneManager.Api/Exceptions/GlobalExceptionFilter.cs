using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using TimeZoneManager.Exceptions;

namespace TimeZoneManager.Api.Exceptions
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private ExceptionToHttpCodeConverter _converter => new ExceptionToHttpCodeConverter();

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            Exception ex = context.Exception.GetInnerAspectInvocationException();

            HttpExceptionResponseInfo info = _converter.GetMessageAndHttpCode(ex);

            context.HttpContext.Response.StatusCode = (int)info.Status;
            if (info.Message != null)
            {
                context.Result = new JsonResult(info.Message);
            }

            _logger.LogError(
                $"Exception mapped to:{Environment.NewLine}" +
                $"* HTTP {context.HttpContext.Response.StatusCode}: {info.Status.ToString()}{Environment.NewLine}" +
                $"* Message: \"{info.Message}\"");
        }
    }
}
