using System.Collections.Generic;

namespace TimeZoneManager.Authorization
{
    public interface ISession
    {
        string Username { get; }

        IEnumerable<string> Permissions { get; }

        string Token { get; }

        bool IsAuthenticated();

        bool IsAdmin();
    }
}
