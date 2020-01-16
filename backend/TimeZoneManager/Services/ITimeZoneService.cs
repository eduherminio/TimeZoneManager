using System.Collections.Generic;
using TimeZoneManager.Authorization;
using TimeZoneManager.Dto;
using TimeZoneManager.Exceptions;
using TimeZoneManager.Logs;

namespace TimeZoneManager.Services
{
    [Log]
    [ExceptionManagement]
    public interface ITimeZoneService
    {
        [Authorization(PermissionName.TimeZoneCreate)]
        TimeZoneDto Create(TimeZoneDto dto);

        [Authorization(PermissionName.TimeZoneUpdate)]
        TimeZoneDto Update(TimeZoneDto dto);

        [Authorization(PermissionName.TimeZoneRead)]
        ICollection<TimeZoneDto> LoadAll();

        [Authorization(PermissionName.TimeZoneRead)]
        ICollection<TimeZoneDto> FindByName(string name);

        [Authorization(PermissionName.TimeZoneDelete)]
        void Delete(string key);
    }
}
