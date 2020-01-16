using Microsoft.AspNetCore.Mvc;
using TimeZoneManager.Api.Authentication;
using TimeZoneManager.Services;

namespace TimeZoneManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password">Encrypted password</param>
        /// <returns></returns>
        [HttpPost]
        public string Index(string username, string password)
        {
            return _authenticationService.Authenticate(username, password);
        }

        /// <summary>
        /// Renew authentication (required to be logged in)
        /// </summary>
        /// <returns></returns>
        [JwtTokenRequired]
        [HttpPost("renew")]
        public string RenewToken()
        {
            return _authenticationService.RenewToken(HttpContext.Request.Headers["Authorization"]);
        }
    }
}
