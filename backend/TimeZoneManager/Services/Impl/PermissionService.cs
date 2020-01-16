using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TimeZoneManager.Authorization;
using TimeZoneManager.Constants;
using TimeZoneManager.Dao;
using TimeZoneManager.Dto;
using TimeZoneManager.Model;

namespace TimeZoneManager.Services.Impl
{
    public class PermissionService
        : BaseEntityService<Permission, PermissionDto, IPermissionDao>, IPermissionService
    {
        public PermissionService(IPermissionDao entityDao, IMapper mapper)
            : base(entityDao, mapper)
        {
        }

        public override ICollection<PermissionDto> LoadAll()
        {
            return base.LoadAll().ToList();
        }

        protected override bool IsValid(PermissionDto dto)
        {
            return !string.IsNullOrWhiteSpace(dto.Name);
        }

        #region Initialize

        public void Initialize()
        {
            ICollection<string> existingPermissionNames =
                _entityDao.FindByName(DefaultPermissions.AllPermissionsList.Select(p => p.Name).ToList())
                .Select(loadedPermission => loadedPermission.Name).ToList();

            Create(DefaultPermissions.AllPermissionsList
                .Where(contantPermission => !existingPermissionNames.Contains(contantPermission.Name)).ToList());

            if (_entityDao is IInternalPermissionDao internalDao
                && internalDao.InternalFind(DefaultPermissions.SuperAdmin.Name) == null)
            {
                Create(DefaultPermissions.SuperAdmin);
            }
        }

        #endregion
    }
}
