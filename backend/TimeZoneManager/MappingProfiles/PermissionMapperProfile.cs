using TimeZoneManager.Dto;
using TimeZoneManager.Model;

namespace TimeZoneManager.MappingProfiles
{
    public class PermissionMapperProfile : BaseMapperProfile
    {
        public PermissionMapperProfile()
        {
            CreateMap<Permission, PermissionDto>();
            CreateMap<PermissionDto, Permission>()
                .ForMember(destination => destination.Roles, opt => opt.Ignore())
                .ForMember(destination => destination.PermissionInRoles, opt => opt.Ignore());
        }
    }
}
