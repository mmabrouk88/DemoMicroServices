
using CommandService.API.AsyncDataServices;
using CommandService.API.Data;
using CommandService.API.EvenetProcessing;
using Microsoft.EntityFrameworkCore;

namespace CommandService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));
            builder.Services.AddScoped<ICommandRepo, CommandRepo>();
            builder.Services.AddControllers();
          //  builder.Services.AddHostedService<SampleBackgroundService>();
            builder.Services.AddHostedService<MessageBusSubscriber>();
            builder.Services.AddSingleton<IEventProcessor,EventProcessor>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
               
            }

         //   app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}