using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TimeZoneManager.Dao;
using TimeZoneManager.Dto;
using TimeZoneManager.Exceptions;
using TimeZoneManager.Extensions;
using TimeZoneManager.Services;
using Xunit;

namespace TimeZoneManager.Test
{
    public class RoleServiceTest : BaseUnitTest
    {
        private IRoleService _service;
        private IPermissionService _permissionService;

        public RoleServiceTest()
        {
            RenewServices();
        }

        protected override void RenewServices()
        {
            _service = GetService<IRoleService>();
            _permissionService = GetService<IPermissionService>();
        }

        [Fact]
        public void CreateAndLoad()
        {
            // Arrange
            var permission = new PermissionDto { Name = "p" };

            var createdPermission = _permissionService.Create(permission);

            var dto = new RoleDto
            {
                Name = "role",
                Description = "a role"
            };

            dto.Permissions.Add(createdPermission);

            // Act
            var createdDto = _service.Create(dto);
            Assert.NotNull(createdDto);
            Assert.NotEmpty(createdDto.Permissions);

            // Assert
            NewContext();
            var loadedDto = _service.Load(createdDto.Key);
            Assert.NotNull(loadedDto);
            Assert.Equal(dto.Name, loadedDto.Name);
            Assert.Equal(dto.Description, loadedDto.Description);
            Assert.Subset(
                dto.Permissions.Select(p => p.Name).ToHashSet(),
                loadedDto.Permissions.Select(p => p.Name).ToHashSet());
        }

        [Fact]
        public void LoadAll()
        {
            // Arrange
            var dtos = new List<RoleDto>
            {
                new RoleDto { Name = "role", Description = "a role" },
                new RoleDto { Name  = "role2", Description = "another role" }
            };

            dtos.ForEach(dto => _service.Create(dto));

            // Act
            NewContext();
            var loadedUsers = _service.LoadAll();

            // Assert
            Assert.NotEmpty(loadedUsers);
            Assert.Equal(dtos.Count, loadedUsers.Count);
            Assert.Subset(dtos.Select(u => u.Name).ToHashSet(), loadedUsers.Select(u => u.Name).ToHashSet());
        }

        [Fact]
        public void ShouldNotCreateDuplicated()
        {
            // Arrange
            var dto = new RoleDto
            {
                Name = "role",
                Description = "a role"
            };
            _service.Create(dto);

            // Act & assert
            NewContext();
            Assert.Throws<DbUpdateException>(() => _service.Create(dto));
        }

        [Fact]
        public void Update()
        {
            // Arrange
            var permission1 = new PermissionDto { Name = "p1" };
            var permission2 = new PermissionDto { Name = "p2" };
            var permission3 = new PermissionDto { Name = "p3" };
            var permission4 = new PermissionDto { Name = "p4" };

            var createdPermission1 = _permissionService.Create(permission1);
            var createdPermission2 = _permissionService.Create(permission2);
            var createdPermission3 = _permissionService.Create(permission3);
            var createdPermission4 = _permissionService.Create(permission4);

            var dto = new RoleDto
            {
                Name = "role",
                Description = "a role"
            };

            dto.Permissions.AddRange(new[] { createdPermission1, createdPermission2 });
            var createdDto = _service.Create(dto);

            // Act
            NewContext();
            createdDto.Description += createdDto.Description;
            createdDto.Permissions.Remove(createdDto.Permissions.Single(p => p.Key == createdPermission2.Key));
            createdDto.Permissions.AddRange(new[] { createdPermission3, createdPermission4 });
            var updatedDto = _service.Update(createdDto);
            Assert.NotNull(updatedDto);

            // Assert
            NewContext();
            var loadedUser = _service.Load(createdDto.Key);
            Assert.Equal(createdDto.Description, loadedUser.Description);
            Assert.Equal(3, loadedUser.Permissions.Count);
            Assert.Subset(
                new HashSet<string>(new[] { createdPermission1, createdPermission3, createdPermission4 }.Select(p => p.ToString())),
                new HashSet<string>(loadedUser.Permissions.Select(p => p.ToString())));
        }

        [Fact]
        public void ShouldNotUpdateIfRoleNameIsChange()
        {
            // Arrange
            var dto = new RoleDto
            {
                Name = "role",
                Description = "a role"
            };

            var createdDto = _service.Create(dto);

            // Act && assert
            NewContext();
            createdDto.Name += "*";
            Assert.Throws<InternalErrorException>(() => _service.Update(createdDto));

            NewContext();
            var dao = GetService<IRoleDao>();
            Assert.NotNull(_service.Load(createdDto.Key));
            Assert.NotNull(dao.FindWhere(r => r.Name == dto.Name));
        }

        [Fact]
        public void Delete()
        {
            // Arrange
            var dto = new RoleDto
            {
                Name = "role",
                Description = "a role"
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
    }
}
