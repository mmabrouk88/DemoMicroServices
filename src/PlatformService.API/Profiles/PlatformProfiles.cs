using AutoMapper;
using PlatformService.API.DTOs;
using PlatformService.API.Models;

namespace PlatformService.API.Profiles
{
    public class PlatformProfiles : Profile
    {
        public PlatformProfiles()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<PlatformCreateDTO, Platform>();
            CreateMap<PlatformReadDTO, PlatformPublishedDto>();
            CreateMap<Platform, GrpcPlatformModel>()
              .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));
        }

    }
}
