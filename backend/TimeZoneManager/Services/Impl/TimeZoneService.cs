using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TimeZoneManager.Authorization;
using TimeZoneManager.Dao;
using TimeZoneManager.Dto;
using TimeZoneManager.Exceptions;
using TimeZoneManager.Model;

namespace TimeZoneManager.Services.Impl
{
    public class TimeZoneService
        : ITimeZoneService
    {
        private readonly ITimeZoneDao _entityDao;
        private readonly IUserDao _userDao;
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public TimeZoneService(ITimeZoneDao entityDao, IUserDao userDao, ISession session, IMapper mapper)
        {
            _entityDao = entityDao;
            _userDao = userDao;
            _session = session;
            _mapper = mapper;
        }

        public TimeZoneDto Create(TimeZoneDto dto)
        {
            if (!IsValid(dto))
            {
                throw new InvalidDataException("Ill formed object");
            }
            if (_entityDao.Any(entity => entity.Name == dto.Name))
            {
                throw new EntityAlreadyExistsException($"An entity with name {dto.Name} already exists in the system");
            }

            TimeZone entity = _mapper.Map<TimeZone>(dto);

            User user = _userDao.FindByUsername(_session.Username);
            entity.UserId = user.Id;
            entity.User = user;

            TimeZone entityCreated = _entityDao.Create(entity);

            return _mapper.Map<TimeZoneDto>(entityCreated);
        }

        public ICollection<TimeZoneDto> FindByName(string name)
        {
            var candidateTimeZones = string.IsNullOrWhiteSpace(name)
                ? _entityDao.LoadAll()
                : _entityDao.FindByName(name);

            var authorizedTimeZones = candidateTimeZones.Where(IsUserAuthorized);

            return authorizedTimeZones
                .Select(tz => _mapper.Map<TimeZoneDto>(tz))
                .ToList();
        }

        public ICollection<TimeZoneDto> LoadAll()
        {
            var candidateTimeZones = _entityDao.LoadAll();

            var authorizedTimeZones = candidateTimeZones.Where(IsUserAuthorized);

            return authorizedTimeZones
                .Select(tz => _mapper.Map<TimeZoneDto>(tz))
                .ToList();
        }

        public void Delete(string key)
        {
            var timeZone = _entityDao.Load(key);

            if (!IsUserAuthorized(timeZone))
            {
                throw new EntityDoesNotExistException($"There's not such a TimeZone");
            }

            _entityDao.Delete(timeZone);
        }

        public TimeZoneDto Update(TimeZoneDto dto)
        {
            var timeZone = _entityDao.Load(dto.Key);

            if (!IsUserAuthorized(timeZone))
            {
                throw new EntityDoesNotExistException($"There's not such a TimeZone");
            }
            timeZone = _mapper.Map(dto, timeZone);
            var updatedTimeZone = _entityDao.Update(timeZone);

            return _mapper.Map<TimeZoneDto>(updatedTimeZone);
        }

        private bool IsUserAuthorized(TimeZone timeZone)
        {
            return _session.Permissions.Contains(nameof(PermissionName.TimeZoneAdmin))
                || _session.Permissions.Contains(AdminPermissions.AdminPermissionName)
                || _session.Permissions.Contains(AdminPermissions.SuperAdminPermissionName)
                || timeZone.User.Username == _session.Username;
        }

        private static bool IsValid(TimeZoneDto dto)
        {
            return !string.IsNullOrEmpty(dto.CityName)
                && dto.GmtDifferenceInHours != int.MinValue
                && !string.IsNullOrEmpty(dto.Name);
        }
    }
}
