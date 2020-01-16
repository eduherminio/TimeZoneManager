using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeZoneManager.Authorization;
using TimeZoneManager.Constants;
using TimeZoneManager.Dto;
using TimeZoneManager.Exceptions;
using TimeZoneManager.Services;
using TimeZoneManager.Services.Impl;
using Xunit;

namespace TimeZoneManager.Test
{
    public class TimeZoneTest : BaseUnitTest
    {
        private ITimeZoneService _timeZoneService;

        public TimeZoneTest()
        {
            RenewServices();
            IDataInitializationService dataInitialization = new DataInitializationService(ServiceProvider);
            dataInitialization.Initialize();
        }

        protected override void RenewServices()
        {
            _timeZoneService = GetService<ITimeZoneService>();
        }

        [Fact]
        public void Create()
        {
            // Arrange
            var dto = GenerateTimeZone();
            ((Session)Session).Username = DefaultUsers.TimeZoneManager.Username;

            // Act
            var createdDto = _timeZoneService.Create(dto);
            Assert.NotNull(createdDto);

            // Assert
            NewContext();
            var loadedDto = _timeZoneService.FindByName(dto.Name).Single();

            Assert.Equal(dto.Name, loadedDto.Name);
            Assert.Equal(dto.CityName, loadedDto.CityName);
            Assert.Equal(dto.GmtDifferenceInHours, loadedDto.GmtDifferenceInHours);
            Assert.Equal(DefaultUsers.TimeZoneManager.Username, loadedDto.OwnerUsername);
        }

        [Fact]
        public void ShouldNotCreateDuplicated()
        {
            // Arrange
            var dto = GenerateTimeZone();
            ((Session)Session).Username = DefaultUsers.TimeZoneManager.Username;

            var createdDto = _timeZoneService.Create(dto);
            Assert.NotNull(createdDto);

            // Act && Assert
            NewContext();
            Assert.Throws<EntityAlreadyExistsException>(() => _timeZoneService.Create(dto));
        }

        [Fact]
        public void LoadAll()
        {
            // Arrange
            var userDto = GenerateTimeZone();
            ((Session)Session).Username = DefaultUsers.TimeZoneManager.Username;
            ((Session)Session).Permissions = DefaultRoles.TimeZoneManager.Permissions.Select(p => p.Name);
            var createdUserDto = _timeZoneService.Create(userDto);
            Assert.NotNull(createdUserDto);

            NewContext();
            var adminDto = GenerateTimeZone();
            ((Session)Session).Username = DefaultUsers.Admin.Username;
            ((Session)Session).Permissions = DefaultRoles.Admin.Permissions.Select(p => p.Name);
            var createdAdminDto = _timeZoneService.Create(adminDto);
            Assert.NotNull(createdAdminDto);

            // Act and assert
            NewContext();
            ((Session)Session).Username = DefaultUsers.TimeZoneManager.Username;
            ((Session)Session).Permissions = DefaultRoles.TimeZoneManager.Permissions.Select(p => p.Name);
            var loadedUserDto = _timeZoneService.LoadAll().Single();
            Assert.Equal(createdUserDto.Key, loadedUserDto.Key);

            NewContext();
            ((Session)Session).Username = DefaultUsers.Admin.Username;
            ((Session)Session).Permissions = DefaultRoles.Admin.Permissions.Select(p => p.Name);
            var loadedAdminDtos = _timeZoneService.LoadAll();
            Assert.Subset(new HashSet<string>(new[] { createdUserDto.Name, createdAdminDto.Name }),
                new HashSet<string>(loadedAdminDtos.Select(dto => dto.Name)));
        }

        [Fact]
        public void FindByName()
        {
            // Arrange
            var userDto = GenerateTimeZone();
            ((Session)Session).Username = DefaultUsers.TimeZoneManager.Username;
            ((Session)Session).Permissions = DefaultRoles.TimeZoneManager.Permissions.Select(p => p.Name);
            var createdUserDto = _timeZoneService.Create(userDto);
            Assert.NotNull(createdUserDto);

            NewContext();
            var adminDto = GenerateTimeZone();
            var adminDto2 = GenerateTimeZone();
            var adminDto3 = GenerateTimeZone();
            var adminDto4 = GenerateTimeZone();
            adminDto2.Name = $"{adminDto.Name}2";
            adminDto3.Name = $"{adminDto.Name}3";
            ((Session)Session).Username = DefaultUsers.Admin.Username;
            ((Session)Session).Permissions = DefaultRoles.Admin.Permissions.Select(p => p.Name);
            var createdAdminDto = _timeZoneService.Create(adminDto);
            var createdAdminDto2 = _timeZoneService.Create(adminDto2);
            var createdAdminDto3 = _timeZoneService.Create(adminDto3);
            var createdAdminDto4 = _timeZoneService.Create(adminDto4);
            Assert.NotNull(createdAdminDto);
            Assert.NotNull(createdAdminDto2);
            Assert.NotNull(createdAdminDto3);
            Assert.NotNull(createdAdminDto4);

            // Act and assert
            NewContext();
            ((Session)Session).Username = DefaultUsers.TimeZoneManager.Username;
            ((Session)Session).Permissions = DefaultRoles.TimeZoneManager.Permissions.Select(p => p.Name);
            var loadedUserDto = _timeZoneService.FindByName(userDto.Name).Single();
            Assert.Equal(createdUserDto.Key, loadedUserDto.Key);

            NewContext();
            ((Session)Session).Username = DefaultUsers.Admin.Username;
            ((Session)Session).Permissions = DefaultRoles.Admin.Permissions.Select(p => p.Name);
            var loadedAdminDtos = _timeZoneService.FindByName(userDto.Name);
            Assert.Subset(new HashSet<string>(new[] { createdUserDto.Name, createdAdminDto.Name, createdAdminDto2.Name, createdAdminDto3.Name }),
                new HashSet<string>(loadedAdminDtos.Select(dto => dto.Name)));
            Assert.DoesNotContain(createdAdminDto4.Name, loadedAdminDtos.Select(dto => dto.ToString()));
        }

        [Fact]
        public void Update()
        {
            // Arrange
            var dto = GenerateTimeZone();
            ((Session)Session).Username = DefaultUsers.TimeZoneManager.Username;
            var createdDto = _timeZoneService.Create(dto);
            Assert.NotNull(createdDto);

            // Act
            NewContext();
            ((Session)Session).Username = DefaultUsers.TimeZoneManager.Username;
            ((Session)Session).Permissions = DefaultRoles.Admin.Permissions.Select(p => p.Name);
            createdDto.Name += "*";
            createdDto.CityName += "-";
            ++createdDto.GmtDifferenceInHours;
            createdDto.OwnerUsername = null;
            var updatedDto = _timeZoneService.Update(createdDto);
            Assert.NotNull(updatedDto);

            // Assert
            NewContext();
            var loadedDto = _timeZoneService.FindByName(dto.Name).Single();
            Assert.Equal(dto.Name + "*", loadedDto.Name);
            Assert.Equal(dto.CityName + "-", loadedDto.CityName);
            Assert.Equal(++dto.GmtDifferenceInHours, loadedDto.GmtDifferenceInHours);
            Assert.Equal(DefaultUsers.TimeZoneManager.Username, loadedDto.OwnerUsername);
        }

        [Fact]
        public void ShouldNotUpdateOtherPersonTimezone()
        {
            // Arrange
            var dto = GenerateTimeZone();
            ((Session)Session).Username = DefaultUsers.Admin.Username;
            ((Session)Session).Permissions = DefaultRoles.Admin.Permissions.Select(p => p.Name);
            var createdDto = _timeZoneService.Create(dto);
            Assert.NotNull(createdDto);

            // Act
            NewContext();
            ((Session)Session).Username = DefaultUsers.TimeZoneManager.Username;
            ((Session)Session).Permissions = DefaultRoles.TimeZoneManager.Permissions.Select(p => p.Name);
            createdDto.Name += "*";
            createdDto.CityName += "-";
            ++createdDto.GmtDifferenceInHours;
            createdDto.OwnerUsername = null;
            Assert.Throws<EntityDoesNotExistException>(() => _timeZoneService.Update(createdDto));

            // Assert
            NewContext();
            var loadedDto = _timeZoneService.FindByName(dto.Name).Single();
            Assert.Equal(dto.Name, loadedDto.Name);
            Assert.Equal(dto.CityName, loadedDto.CityName);
            Assert.Equal(dto.GmtDifferenceInHours, loadedDto.GmtDifferenceInHours);
            Assert.Equal(DefaultUsers.Admin.Username, loadedDto.OwnerUsername);
        }

        [Fact]
        public void Delete()
        {
            // Arrange
            var dto = GenerateTimeZone();
            ((Session)Session).Username = DefaultUsers.TimeZoneManager.Username;
            var createdDto = _timeZoneService.Create(dto);
            Assert.NotNull(createdDto);

            // Act
            NewContext();
            ((Session)Session).Username = DefaultUsers.Admin.Username;
            ((Session)Session).Permissions = DefaultRoles.Admin.Permissions.Select(p => p.Name);
            _timeZoneService.Delete(createdDto.Key);

            // Assert
            NewContext();
            Assert.Empty(_timeZoneService.FindByName(dto.Name));
        }

        [Fact]
        public void ShouldNotDeleteOtherPersonTimezone()
        {
            // Arrange
            var dto = GenerateTimeZone();
            ((Session)Session).Username = DefaultUsers.Admin.Username;
            var createdDto = _timeZoneService.Create(dto);
            Assert.NotNull(createdDto);

            // Act
            NewContext();
            ((Session)Session).Username = DefaultUsers.TimeZoneManager.Username;
            ((Session)Session).Permissions = DefaultRoles.TimeZoneManager.Permissions.Select(p => p.Name);
            Assert.Throws<EntityDoesNotExistException>(() => _timeZoneService.Delete(createdDto.Key));

            // Assert
            NewContext();
            Assert.Equal(createdDto.Key,
                _timeZoneService.FindByName(dto.Name).Single().Key);
        }

        private static TimeZoneDto GenerateTimeZone()
        {
            return new TimeZoneDto()
            {
                Name = Guid.NewGuid().ToString(),
                CityName = Guid.NewGuid().ToString(),
                GmtDifferenceInHours = 5
            };
        }
    }
}
