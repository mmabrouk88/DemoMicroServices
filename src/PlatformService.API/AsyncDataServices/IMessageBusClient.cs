using PlatformService.API.DTOs;

namespace PlatformService.API.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDto publishedDto);
    }
}
