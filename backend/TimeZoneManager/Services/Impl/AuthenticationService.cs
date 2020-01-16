using System;
using TimeZoneManager.Authentication;
using TimeZoneManager.Dao;
using TimeZoneManager.Model;

namespace TimeZoneManager.Services.Impl
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserDao _userDao;
        private readonly IJwtManager _jwtManager;

        public AuthenticationService(IUserDao userDao, IJwtManager jwtManager)
        {
            _userDao = userDao;
            _jwtManager = jwtManager;
        }

        public string Authenticate(string username, string password)
        {
            User user = _userDao.LoadWithPermissions(username);
            if (user != null && user.Password.ToUpperInvariant() == password?.ToUpperInvariant() && user.GetPermissionNames().Count > 0)
            {
                return GenerateTokenForUser(user);
            }

            throw new UnauthorizedAccessException("Wrong credentials");
        }

        public string RenewToken(string authenticationHeader)
        {
            JwtTokenPayload payload = _jwtManager.GetPayload(authenticationHeader);
            User user = _userDao.LoadWithPermissions(payload.Username);

            return GenerateTokenForUser(user);
        }

        private string GenerateTokenForUser(User user)
        {
            JwtTokenPayload payload = new JwtTokenPayload()
            {
                Username = user.Username,
                Permissions = user.GetPermissionNames(),
            };

            return _jwtManager.GenerateToken(payload);
        }
    }
}
