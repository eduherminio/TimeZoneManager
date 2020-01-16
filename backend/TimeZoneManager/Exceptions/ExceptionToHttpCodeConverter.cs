using System;
using System.Collections.Generic;
using System.Net;

namespace TimeZoneManager.Exceptions
{
    public class ExceptionToHttpCodeConverter
    {
        private readonly Dictionary<Type, HttpExceptionResponseInfo> _exceptionInfo = new Dictionary<Type, HttpExceptionResponseInfo>();

        protected void AddValues(Type type, HttpStatusCode code, string msg)
        {
            _exceptionInfo.Add(type, new HttpExceptionResponseInfo(code, msg));
        }

        private HttpExceptionResponseInfo GetValue(Type type)
        {
            if (!_exceptionInfo.TryGetValue(type, out HttpExceptionResponseInfo info))
            {
                info = new HttpExceptionResponseInfo(HttpStatusCode.InternalServerError, null);
            }

            return info;
        }

        public ExceptionToHttpCodeConverter()
        {
            AddValues(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized, "Unauthorized access");
            AddValues(typeof(InvalidOperationException), HttpStatusCode.BadRequest, string.Empty);
            AddValues(typeof(EntityDoesNotExistException), HttpStatusCode.NotFound, string.Empty);
            AddValues(typeof(InternalErrorException), HttpStatusCode.InternalServerError, string.Empty);
        }

        public HttpExceptionResponseInfo GetMessageAndHttpCode(Exception ex)
        {
            var values = GetValue(ex.GetType());

            if (string.IsNullOrEmpty(values.Message))
            {
                values.Message = ex.Message;
            }

            return values;
        }
    }
}
