using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TimeZoneManager.Logs;

namespace TimeZoneManager.Exceptions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class ExceptionManagementAttribute : AbstractInterceptorAttribute
    {
        [FromServiceContext]
#pragma warning disable RCS1170 // Use read-only auto-implemented property. - Public set needed for DI
        private ILogger<ExceptionManagementAttribute> Logger { get; set; }
#pragma warning restore RCS1170 // Use read-only auto-implemented property.

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                await next(context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleException(e, context);
            }
        }

        private void HandleException(Exception e, AspectContext context)
        {
            if (e is AspectInvocationException aspectCoreException)
            {
                if (e.InnerException != null)
                {
                    HandleException(aspectCoreException.InnerException, context);
                }

                LogError("AspectCore exception", context, e.Message);
                throw e;
            }
            if (e is DbUpdateException dbUpdateException)
            {
                if (e.InnerException != null)
                {
                    LogError("Database exception", context, dbUpdateException.InnerException.Message);
                    throw new DatabaseException(dbUpdateException.InnerException.Message);
                }
                else
                {
                    LogError("Database exception", context, e.Message);
                    throw new DatabaseException(e.Message);
                }
            }
            if (e is BaseCustomException customException)
            {
                LogError(customException.Description, context, customException.Message);
                throw e;
            }
            if (e is UnauthorizedAccessException)
            {
                LogError("Unauthorized access", context, e.Message);
                throw e;
            }
            if (e is InvalidOperationException)
            {
                LogError("Invalid operation", context, e.Message);
                throw e;
            }

            HandleGenericException(e, context);
        }

        private void LogError(string type, AspectContext context, string message)
        {
            string parametersInStringFormat = context.Parameters.Length > 0
                ? LogHelpers.ValueToLog(context.Parameters)
                : "No params";
            Logger.LogError($"{type} " +
                $"- {context.ImplementationMethod.ReflectedType.FullName}" +
                $".{context.ProxyMethod.Name} - {parametersInStringFormat}: {message}");
        }

        private void HandleGenericException(Exception e, AspectContext context)
        {
            LogError("Exception", context, e.Message);
            throw new InternalErrorException(e.Message, e.InnerException);
        }
    }
}
