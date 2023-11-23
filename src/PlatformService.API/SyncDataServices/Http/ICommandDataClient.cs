using PlatformService.API.DTOs;

namespace PlatformService.API.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDTO plat);
    }
}
