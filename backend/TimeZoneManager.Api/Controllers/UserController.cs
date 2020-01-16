using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TimeZoneManager.Api.Authentication;
using TimeZoneManager.Dto;
using TimeZoneManager.Services;

namespace TimeZoneManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Allows the registration of a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public UserDto Register([FromBody]FullUserDto user)
        {
            return _userService.Register(user);
        }

        /// <summary>
        /// Creates a User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [JwtTokenRequired]
        public UserDto Create([FromBody]FullUserDto user)
        {
            return _userService.Create(user);
        }

        /// <summary>
        /// Loads a User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [JwtTokenRequired]
        public UserDto Load(string id)
        {
            return _userService.Load(id);
        }

        /// <summary>
        /// Finds Users that match a given name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("search/{id}")]
        [JwtTokenRequired]
        public ICollection<UserDto> FindByName(string id)
        {
            return _userService.FindByName(id);
        }

        /// <summary>
        /// Loads all Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [JwtTokenRequired]
        public ICollection<UserDto> LoadAll()
        {
            return _userService.LoadAll();
        }

        /// <summary>
        /// Updates a User
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [JwtTokenRequired]
        public UserDto Update([FromBody]UserDto dto)
        {
            return _userService.Update(dto);
        }

        /// <summary>
        /// Deletes a User
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [JwtTokenRequired]
        public void Delete(string id)
        {
            _userService.Delete(id);
        }
    }
}
