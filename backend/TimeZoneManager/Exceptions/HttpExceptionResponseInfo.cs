using System.Net;

namespace TimeZoneManager.Exceptions
{
    public class HttpExceptionResponseInfo
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

        public HttpExceptionResponseInfo(HttpStatusCode status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}
