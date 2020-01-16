using TimeZoneManager.Authorization;
using TimeZoneManager.Exceptions;
using TimeZoneManager.Logs;

namespace TimeZoneManager.Services
{
    [Log]
    [ExceptionManagement]
    public interface IAuthenticationService
    {
        string Authenticate(string username, string password);

        [Authorization]
        string RenewToken(string authenticationHeader);
    }
}
