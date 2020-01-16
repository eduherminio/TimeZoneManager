using AutoMapper;
using TimeZoneManager.Authorization;
using TimeZoneManager.Dto;
using TimeZoneManager.Model;

namespace TimeZoneManager.MappingProfiles
{
    public class TimeZoneMapperProfile : BaseMapperProfile
    {
        public TimeZoneMapperProfile()
        {
            CreateMap<TimeZone, TimeZoneDto>()
                .ForMember(destination => destination.OwnerUsername, cfg => cfg.MapFrom(source => source.User.Username));
            CreateMap<TimeZoneDto, TimeZone>()
                .ForMember(destination => destination.User, cfg => cfg.Ignore())
                .ForMember(destination => destination.UserId, cfg => cfg.Ignore());
        }
    }
}
