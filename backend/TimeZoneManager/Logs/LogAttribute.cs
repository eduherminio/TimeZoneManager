using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;

using System;
using AspectCore.DependencyInjection;

namespace TimeZoneManager.Logs
{
    /// <summary>
    /// This class is an Interceptor to log all method invocations
    /// </summary>
    /// <remarks>
    /// The default LogLevel is Trace
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class LogAttribute : AbstractInterceptorAttribute
    {
        [FromServiceContext]
#pragma warning disable RCS1170 // Use read-only auto-implemented property. - Public set needed for DI
        public ILogger<LogAttribute> Logger { get; set; }
#pragma warning restore RCS1170 // Use read-only auto-implemented property.

        private readonly LogLevel _logLevel;

        public LogAttribute()
        {
            _logLevel = LogLevel.Debug;
        }

        public LogAttribute(LogLevel logLevel)
        {
            _logLevel = logLevel;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            string parametersInStringFormat = context.Parameters.Length > 0
                ? LogHelpers.ValueToLog(context.Parameters)
                : "No params";

            Logger.Log(_logLevel,
                "Invocation to: {0} : {1} with params: {2}",
                context.Implementation.GetType(),
                context.ImplementationMethod.Name, parametersInStringFormat);

            await next(context).ConfigureAwait(false);

            Logger.Log(_logLevel,
                "Return value: {0}",
                context.ReturnValue != null
                    ? LogHelpers.ValueToLog(context.ReturnValue)
                    : "void");
        }
    }
}
