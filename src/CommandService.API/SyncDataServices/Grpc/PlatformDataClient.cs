using AutoMapper;
using CommandService.API.Models;
using Grpc.Core;
using Grpc.Net.Client;
using PlatformService.API.Protos;
using System.Threading.Channels;

namespace CommandService.API.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            _mapper=mapper;
            _configuration=configuration;
        }
        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcPlatform"]}");
            // var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
            //// GrpcChannel channel = new GrpcChannel("localhost:50051", ChannelCredentials.Insecure);

            // var client = new GrpcPlatform.GrpcPlatformClient(channel);
            // var request = new GetAllRequest();

            // try
            // {
            //     var reply = client.GetAllPlatforms(request);
            //     return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
            //     return null;
            // }
            var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"], new GrpcChannelOptions
            {
              //  Credentials = ChannelCredentials.Insecure, // Use Insecure for development, switch to secure credentials in production
                LoggerFactory = LoggerFactory.Create(builder => builder.AddConsole()), // Optional: Add logging to see more details
            });

            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                Console.WriteLine($"Received Platforms Using Grpc");
                Console.WriteLine($"{reply.Platform[0].Name}");
                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"--> Could not call gRPC Server. Status code: {ex.StatusCode}, Status detail: {ex.Status.Detail}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> An unexpected error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
