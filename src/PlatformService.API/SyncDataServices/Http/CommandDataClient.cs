using PlatformService.API.DTOs;
using System.Text.Json;
using System.Text;

namespace PlatformService.API.SyncDataServices.Http
{
    public class CommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CommandDataClient(HttpClient httpClient,IConfiguration
            configuration)
        {
            _httpClient=httpClient;
            _configuration=configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDTO plat)
        {
            var httpContent = new StringContent(
                  JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync(_configuration["CommandService"],httpContent);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"--> Sync POST to CommandService From Platform Service using: {_configuration["CommandService"]} was OK!");
            }
            else
            {
                Console.WriteLine($" Sync POST to CommandService From Platform Service using: {_configuration["CommandService"]} was NOT OK!");
            }
        }
    }
}
