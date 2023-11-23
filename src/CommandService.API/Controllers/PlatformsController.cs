using AutoMapper;
using CommandService.API.Data;
using CommandService.API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.API.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : Controller
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<PlatformreadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from Our Command Service API");
            var platformItems = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformreadDto>>(platformItems));
        }

        [HttpPost]
        public IActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");
            return Ok("Inbound test from platform controller in Command Service ");
        }
    }
}
