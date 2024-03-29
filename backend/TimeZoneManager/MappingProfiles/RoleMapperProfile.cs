﻿using AutoMapper;
using System.Linq;
using TimeZoneManager.Dao;
using TimeZoneManager.Dto;
using TimeZoneManager.Extensions;
using TimeZoneManager.Model;

namespace TimeZoneManager.MappingProfiles
{
    public class RoleMapperProfile : BaseMapperProfile
    {
        public RoleMapperProfile()
        {
            // Entity --> DTO
            CreateMap<Role, RoleDto>();

            // DTO --> Entity
            CreateMap<RoleDto, Role>()
                .ForMember(destination => destination.Permissions, opt => opt.Ignore())
                .ForMember(destination => destination.PermissionInRoles, opt => opt.Ignore())
                .ForMember(destination => destination.Users, opt => opt.Ignore())
                .ForMember(destination => destination.RoleInUsers, opt => opt.Ignore())
                .ConstructUsing(_ => new Role())
                .AfterMap<TrackRoleAction>();
        }
    }

    public class TrackRoleAction : IMappingAction<RoleDto, Role>
    {
        private readonly IPermissionDao _permissionDAO;

        public TrackRoleAction(IPermissionDao permissionDao)
        {
            _permissionDAO = permissionDao;
        }

        public void Process(RoleDto source, Role destination, ResolutionContext _)
        {
            TrackPermissions(source, destination);
        }

        private void TrackPermissions(RoleDto source, Role destination)
        {
            if (destination.Id != default)
            {
                destination.Permissions.RemoveAll(permission => !source.Permissions.Any(PermissionDTO => PermissionDTO.Key == permission.AutoGeneratedKey));
            }

            var newPermissionKeys = source.Permissions
                .Where(permission => !destination.Permissions.Any(permissionInDb => permission.Key == permissionInDb.AutoGeneratedKey))
                .Select(permission => permission.Key)
                .ToList();

            destination.Permissions.AddRange(_permissionDAO.Load(newPermissionKeys));
        }
    }
}
