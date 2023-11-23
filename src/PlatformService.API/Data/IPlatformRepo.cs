using PlatformService.API.Models;

namespace PlatformService.API.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAll();
        Platform GetPlatformById(int Id);
        Platform GetPlatformByName(string Name);
        void CreatePlatform(Platform platform);

    }
}
