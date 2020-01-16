using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeZoneManager.Dto;
using TimeZoneManager.Exceptions;
using TimeZoneManager.Services;
using Xunit;

namespace TimeZoneManager.Test
{
    public class PermissionServiceTest : BaseUnitTest
    {
        private IPermissionService _service;

        public PermissionServiceTest()
        {
            RenewServices();
        }

        protected override void RenewServices()
        {
            _service = GetService<IPermissionService>();
        }

        [Fact]
        public void CreateAndLoad()
        {
            // Arrange
            var dto = new PermissionDto
            {
                Name = "permission",
                Description = "a permission"
            };

            // Act
            var createdDto = _service.Create(dto);
            Assert.NotNull(createdDto);

            // Assert
            NewContext();
            var loadedDto = _service.Load(createdDto.Key);
            Assert.NotNull(loadedDto);
            Assert.Equal(dto.Name, loadedDto.Name);
            Assert.Equal(dto.Description, loadedDto.Description);
        }

        [Fact]
        public void LoadAll()
        {
            // Arrange
            var dtos = new List<PermissionDto>
            {
                new PermissionDto { Name = "permission", Description = "a permission" },
                new PermissionDto { Name = "permission2", Description = "another permission" }
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
            var dto = new PermissionDto
            {
                Name = "permission",
                Description = "a permission"
            };
            _service.Create(dto);

            // Act & assert
            NewContext();
            Assert.Throws<DbUpdateException>(() => _service.Create(dto));
        }
    }
}
