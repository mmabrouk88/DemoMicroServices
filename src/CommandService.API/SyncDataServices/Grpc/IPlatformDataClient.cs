using CommandService.API.Models;

namespace CommandService.API.SyncDataServices.Grpc
{
    public interface IPlatformDataClient
    {
        IEnumerable<Platform> ReturnAllPlatforms();
    }
}
