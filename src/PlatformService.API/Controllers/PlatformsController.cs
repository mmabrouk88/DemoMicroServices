using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.API.AsyncDataServices;
using PlatformService.API.Data;
using PlatformService.API.DTOs;
using PlatformService.API.Models;
using PlatformService.API.SyncDataServices.Http;

namespace PlatformService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ILogger<PlatformsController> _logger;
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(
            ILogger<PlatformsController> logger,
            IMapper mapper,
            IPlatformRepo repo,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient
            )
        {
            _logger = logger;
            _repo=repo;
            _mapper = mapper;
            _commandDataClient=commandDataClient;
            _messageBusClient = messageBusClient;
        }
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms....");

            var platformItems = _repo.GetAll();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platformItems));
        }
        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDTO> GetPlatformById(int id)
        {
            var platformItem = _repo.GetPlatformById(id);
            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDTO>(platformItem));
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult<PlatformReadDTO>> CreatePlatform(PlatformCreateDTO PlatformCreateDTO)
        {
            var platformModel = _mapper.Map<Platform>(PlatformCreateDTO);
            _repo.CreatePlatform(platformModel);
            _repo.SaveChanges();

            var PlatformReadDTO = _mapper.Map<PlatformReadDTO>(platformModel);
            // Send Sync Message
            try
            {
                await _commandDataClient.SendPlatformToCommand(PlatformReadDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }
            //send Async Message
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(PlatformReadDTO);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"--> Could not send Asynchronously: {ex.Message}");
            }

           
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = PlatformReadDTO.Id }, PlatformReadDTO);
        }

    }
}
