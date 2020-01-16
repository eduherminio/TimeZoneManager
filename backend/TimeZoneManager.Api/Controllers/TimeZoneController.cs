using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TimeZoneManager.Api.Authentication;
using TimeZoneManager.Dto;
using TimeZoneManager.Services;

namespace TimeZoneManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [JwtTokenRequired]
    public class TimeZoneController : ControllerBase
    {
        private readonly ITimeZoneService _timeZoneService;

        public TimeZoneController(ITimeZoneService timeZoneService)
        {
            _timeZoneService = timeZoneService;
        }

        /// <summary>
        /// Creates a TimeZone
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public TimeZoneDto Create([FromBody]TimeZoneDto user)
        {
            return _timeZoneService.Create(user);
        }

        /// <summary>
        /// Finds TimeZones that match a given name
        /// </summary>
        /// <param name="id">TimeZone name</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ICollection<TimeZoneDto> FindByName(string id)
        {
            return _timeZoneService.FindByName(id);
        }

        /// <summary>
        /// Loads all TimeZones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ICollection<TimeZoneDto> LoadAll()
        {
            return _timeZoneService.LoadAll();
        }

        /// <summary>
        /// Updates a TimeZone
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public TimeZoneDto Update([FromBody]TimeZoneDto dto)
        {
            return _timeZoneService.Update(dto);
        }

        /// <summary>
        /// Deletes a TimeZone
        /// </summary>
        /// <param name="id">TimeZone key</param>
        [HttpDelete("id")]
        public void Delete(string id)
        {
            _timeZoneService.Delete(id);
        }
    }
}
