using CommandService.API.Dtos;
using CommandService.API.Models;
using AutoMapper;
using PlatformService.API.Protos;

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
            CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.PlatformId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Commands, opt => opt.Ignore());
        }
    }
}
