using PlatformService.API.DTOs;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Threading.Channels;
using System.Text.Json;
using System.Text;
namespace PlatformService.API.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory() {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                Console.WriteLine("--> Connected to the Message Bus...");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"--> Unable to connect to the Message Bus... {ex.Message}");

            }

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShitdown;
        }

        private void RabbitMQ_ConnectionShitdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }

        public void PublishNewPlatform(PlatformPublishedDto publishedDto)
        {
            var message = JsonSerializer.Serialize(publishedDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("Rabbit MQ connection is open, tring to send message");
                //TO DO SEND MESSAGE TO QUEUE
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("Rabbit MQ connection is closed, NOT sending message!");

            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger",
                        routingKey: "",
                        basicProperties: null,
                        body: body);
            Console.WriteLine($"--> We have sent {message}");
        }
        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
