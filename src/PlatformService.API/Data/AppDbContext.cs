using Microsoft.EntityFrameworkCore;
using PlatformService.API.Models;

namespace PlatformService.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Platform> Platforms { get; set; }
    }
}
