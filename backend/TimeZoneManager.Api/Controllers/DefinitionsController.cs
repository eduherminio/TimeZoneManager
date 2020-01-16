using Microsoft.AspNetCore.Mvc;
using TimeZoneManager.Api.Authentication;

namespace TimeZoneManager.Api.Controllers
{
    [ApiController]
    [JwtTokenRequired]
    [Route("api/[controller]")]
    public class DefinitionsController : ControllerBase
    {
        [HttpGet]
        public Definitions.Constants Index()
        {
            return new Definitions.Constants();
        }
    }
}
