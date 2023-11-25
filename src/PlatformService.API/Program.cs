
using Microsoft.EntityFrameworkCore;
using PlatformService.API.AsyncDataServices;
using PlatformService.API.Data;
using PlatformService.API.SyncDataServices.Gprc;
using PlatformService.API.SyncDataServices.Http;

namespace PlatformService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string endpoint = "";

            // Accessing configuration values
            // Check if the environment is development
            bool useInMemoryDatabase = builder.Environment.IsDevelopment();
            // Add services to the container.
            // builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));

            // Register DbContext
            if (useInMemoryDatabase)
            {
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("InMem"));
                Console.WriteLine("Using In Memory DB");
            }
            else
            {
                // Use SQL Server or another database in other environments
                builder.Services.AddDbContext<AppDbContext>(options =>
                  options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
                Console.WriteLine("Using Sql Server DB");
           }


            builder.Services.AddScoped<IPlatformRepo,PlatformRepo>();
            builder.Services.AddHttpClient<ICommandDataClient,CommandDataClient>();
            builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
            builder.Services.AddControllers();
            builder.Services.AddGrpc();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
           
            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapGrpcService<GrpcPlatformService>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
               
                IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();
               endpoint = configuration["CommandService"];
               PrepDb.PrepPopulation(app, false);
            }
            else
            {
                IConfiguration configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true)
              .Build();
                endpoint = configuration["CommandService"];
            }
            Console.WriteLine($"--> CommandService Endpoint {endpoint}");
            PrepDb.PrepPopulation(app, true);
            app.MapGet("/protos/platforms.proto", async context =>
               {
                   await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
               });
            //app.UseHttpsRedirection();

            app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //    endpoints.MapGrpcService<GrpcPlatformService>();

            //    endpoints.MapGet("/protos/platforms.proto", async context =>
            //    {
            //        await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
            //    });
            //});

            app.MapControllers();

            app.Run();
        }
    }
}