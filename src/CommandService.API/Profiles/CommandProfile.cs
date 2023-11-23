using CommandService.API.Dtos;
using CommandService.API.Models;
using AutoMapper;

namespace CommandService.API.Profiles
{
    public class CommandProfile : Profile
    {
        //Profile Mapping for AutoMapper
        public CommandProfile()
        {
   
            // Source -> Target
            CreateMap<Platform, PlatformreadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));
            
        }
    }
}
