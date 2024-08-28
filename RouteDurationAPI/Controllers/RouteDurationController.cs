using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteDurationAPI.Models;
using RouteDurationAPI.Services;

namespace RouteDurationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteDurationController : ControllerBase
    {
        private readonly IRouteDurationService _routeDurationService;

        public RouteDurationController(IRouteDurationService routeDurationService)
        {
            _routeDurationService = routeDurationService;
        }

        // Punkt kontrolny 1 - rejestracja wjazdu
        [HttpPost]
        [Route("start")]
        public async Task<IActionResult> RegisterVehicleEntry([FromBody] VehicleEntry entry)
        {
            var message = await _routeDurationService.RegisterVehicleEntry(entry);
            return Ok(new { Message = message });
        }

        // Punkt kontrolny 2 - rejestracja wyjazdu
        [HttpPost]
        [Route("end")]
        public async Task<IActionResult> RegisterVehicleExit([FromBody] VehicleEntry exitEntry)
        {
            var (message, averageSpeed) = await _routeDurationService.RegisterVehicleExit(exitEntry);

            if (averageSpeed == null)
            {
                return BadRequest(new { Message = message });
            }

            return Ok(new { Message = message, AverageSpeed = averageSpeed });
        }

        // Czyszczenie pojazdów, które nie pojawiły się w punkcie 2 w 15 minut
        [HttpPost]
        [Route("cleanup")]
        public async Task<IActionResult> CleanupExpiredVehicles([FromQuery] int minutes = 15)
        {
            var count = await _routeDurationService.CleanupExpiredVehicles(minutes);
            return Ok(new { Message = $"{count} vehicles removed after {minutes} minutes." });
        }
    }
}
