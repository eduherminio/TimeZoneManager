using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TimeZoneManager.Authorization;
using TimeZoneManager.Dao;
using TimeZoneManager.Dto;
using TimeZoneManager.Exceptions;
using TimeZoneManager.Extensions;
using TimeZoneManager.Services;
using Xunit;

namespace TimeZoneManager.Test
{
    public class UserServiceTest : BaseUnitTest
    {
        private IUserService _service;
        private IRoleService _roleService;
        private IPermissionService _permissionService;

        public UserServiceTest()
        {
            RenewServices();
        }

        protected override void RenewServices()
        {
            _service = GetService<IUserService>();
            _roleService = GetService<IRoleService>();
            _permissionService = GetService<IPermissionService>();
        }

        [Fact]
        public void CreateAndLoad()
        {
            // Arrange
            var dto = new FullUserDto
            {
                Username = "username",
                Password = "password"
            };

            var permission = new PermissionDto { Name = "p" };
            var createdPermission = _permissionService.Create(permission);

            NewContext();
            var role = new RoleDto { Name = "r" };
            role.Permissions.Add(createdPermission);
            var createdRole = _roleService.Create(role);

            NewContext();
            dto.Roles.Add(createdRole);

            // Act
            var createdDto = _service.Create(dto);
            Assert.NotNull(createdDto);
            Assert.NotEmpty(createdDto.Roles);

            // Assert
            NewContext();
            var loadedDto = _service.Load(createdDto.Key);
            Assert.NotNull(loadedDto);
            Assert.Equal(dto.Username, loadedDto.Username);

            var dao = GetService<IUserDao>();

            var userWithPermissions = dao.LoadWithPermissions(createdDto.Username);
            Assert.NotNull(userWithPermissions);
            Assert.Equal(role.Permissions.Single().Name, userWithPermissions.GetPermissionNames().Single());
        }

        [Fact]
        public void LoadAll()
        {
            // Arrange
            var dtos = new List<FullUserDto>
            {
                new FullUserDto { Username = "username", Password = "password" },
                new FullUserDto { Username = "username2", Password = "password2" }
            };

            dtos.ForEach(dto => _service.Create(dto));

            // Act
            NewContext();
            var loadedUsers = _service.LoadAll();

            // Assert
            Assert.NotEmpty(loadedUsers);
            Assert.Equal(dtos.Count, loadedUsers.Count);
            Assert.Subset(dtos.Select(u => u.Username).ToHashSet(), loadedUsers.Select(u => u.Username).ToHashSet());
        }

        [Fact]
        public void FindByName()
        {
            // Arrange
            var dtos = new List<FullUserDto>
            {
                new FullUserDto { Username = "username", Password = "password" },
                new FullUserDto { Username = "username2", Password = "password2" }
            };

            dtos.ForEach(dto => _service.Create(dto));

            // Act
            NewContext();
            var loadedUsers = _service.FindByName("username2");
            var loadedUsers2 = _service.FindByName("username");

            // Assert
            Assert.Single(loadedUsers);
            Assert.Equal(dtos.Count, loadedUsers2.Count);
        }

        [Fact]
        public void ShouldNotCreateDuplicated()
        {
            // Arrange
            var dto = new FullUserDto
            {
                Username = "username",
                Password = "password"
            };
            _service.Create(dto);

            // Act & assert
            NewContext();
            Assert.Throws<EntityAlreadyExistsException>(() => _service.Create(dto));
        }

        [Fact]
        public void ShouldNotCreateWithoutPassword()
        {
            // Arrange
            var dto = new FullUserDto
            {
                Username = "username",
                Password = null
            };

            // Act & assert
            Assert.Throws<InvalidDataException>(() => _service.Create(dto));
        }

        [Fact]
        public void Update()
        {
            // Arrange
            var role1 = new RoleDto { Name = "r1" };
            var role2 = new RoleDto { Name = "r2" };
            var role3 = new RoleDto { Name = "r3" };
            var role4 = new RoleDto { Name = "r4" };

            var createdRole1 = _roleService.Create(role1);
            var createdRole2 = _roleService.Create(role2);
            var createdRole3 = _roleService.Create(role3);
            var createdRole4 = _roleService.Create(role4);

            var dto = new FullUserDto
            {
                Username = "username",
                Password = "password"
            };

            dto.Roles.AddRange(new[] { createdRole1, createdRole2 });

            var createdDto = _service.Create(dto);

            // Act
            NewContext();
            createdDto.Username += createdDto.Username;
            createdDto.Roles.Remove(createdDto.Roles.Single(p => p.Key == createdRole2.Key));
            createdDto.Roles.AddRange(new[] { createdRole3, createdRole4 });
            var updatedDto = _service.Update(createdDto);
            Assert.NotNull(updatedDto);

            // Assert
            NewContext();
            var loadedUser = _service.Load(createdDto.Key);
            Assert.Equal(createdDto.Username, loadedUser.Username);
            Assert.Equal(3, loadedUser.Roles.Count);
            Assert.Subset(
                new HashSet<string>(new[] { createdRole1, createdRole3, createdRole4 }.Select(p => p.ToString())),
                new HashSet<string>(loadedUser.Roles.Select(p => p.ToString())));
        }

        [Fact]
        public void ShouldNotUpdateIfUsernameAlreadyExists()
        {
            // Arrange
            var dto = new FullUserDto
            {
                Username = "username",
                Password = "password"
            };

            var dto2 = new FullUserDto
            {
                Username = "username2",
                Password = "password"
            };

            var createdDto = _service.Create(dto);
            _service.Create(dto2);

            // Act && assert
            NewContext();
            createdDto.Username = dto2.Username;
            Assert.Throws<DbUpdateException>(() => _service.Update(createdDto));

            NewContext();
            var dao = GetService<IUserDao>();
            Assert.NotNull(_service.Load(createdDto.Key));
            Assert.NotNull(dao.FindWhere(u => u.Username == dto.Username));
            Assert.NotNull(dao.FindWhere(u => u.Username == dto2.Username));
        }

        [Fact]
        public void Delete()
        {
            // Arrange
            var dto = new FullUserDto
            {
                Username = "username",
                Password = "password"
            };

            // Act
            var createdDto = _service.Create(dto);
            NewContext();
            var loadedDto = _service.Load(createdDto.Key);
            Assert.NotNull(loadedDto);

            // Assert
            NewContext();
            _service.Delete(createdDto.Key);
            Assert.Throws<EntityDoesNotExistException>(() => _service.Load(createdDto.Key));
        }

        [Fact]
        public void Register()
        {
            // Arrange
            GetService<IDataInitializationService>()
                .Initialize();

            var dto = new FullUserDto
            {
                Username = "username",
                Password = "password"
            };

            // Act
            var createdDto = _service.Register(dto);
            Assert.NotNull(createdDto);
            Assert.NotEmpty(createdDto.Roles);

            // Assert
            NewContext();
            var loadedDto = _service.Load(createdDto.Key);
            Assert.NotNull(loadedDto);
            Assert.Equal(dto.Username, loadedDto.Username);

            NewContext();
            var loadedRole = _roleService.Load(loadedDto.Roles.Single().Key);
            Assert.Equal(nameof(RoleName.User), loadedRole.Name);
        }

        [Fact]
        public void ShouldNotRegisterWithChosenRoles()
        {
            // Arrange
            GetService<IDataInitializationService>()
                .Initialize();

            var dto = new FullUserDto
            {
                Username = "username",
                Password = "password",
                Roles = new List<RoleDto>()
            };

            dto.Roles.Add(
                new RoleDto
                {
                    Key = GetService<IRoleDao>().FindByName(nameof(RoleName.Admin)).AutoGeneratedKey
                });

            // Act
            var createdDto = _service.Register(dto);
            Assert.NotNull(createdDto);
            Assert.NotEmpty(createdDto.Roles);

            NewContext();
            var loadedDto = _service.Load(createdDto.Key);

            NewContext();
            var loadedRole = _roleService.Load(loadedDto.Roles.Single().Key);
            Assert.Equal(nameof(RoleName.User), loadedRole.Name);
        }
    }
}
