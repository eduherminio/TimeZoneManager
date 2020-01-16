using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeZoneManager.Api.Authentication;
using TimeZoneManager.Dto;
using TimeZoneManager.Services;

namespace TimeZoneManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [JwtTokenRequired]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public ICollection<RoleDto> LoadAll()
        {
            return _roleService.LoadAll();
        }
    }
}
