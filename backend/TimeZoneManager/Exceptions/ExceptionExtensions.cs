using System;

namespace TimeZoneManager.Exceptions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Returns inner exception when given one is from AspectCore, or given one otherwise
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static Exception GetInnerAspectInvocationException(this Exception exception)
        {
            return exception is AspectCore.DynamicProxy.AspectInvocationException && exception.InnerException != null
                ? exception.InnerException
                : exception;
        }
    }
}
