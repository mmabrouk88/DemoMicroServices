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
        }

    }
}
