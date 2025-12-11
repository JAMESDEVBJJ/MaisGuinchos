using MaisGuinchos.Services;
using MaisGuinchos.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaisGuinchos.Controllers
{
    public class MapsController : ControllerBase
    {
        private readonly IMapsService _mapsService;

        public MapsController(IMapsService mapsService)
        {
            _mapsService = mapsService;
        }

        [HttpGet("cords")]
        public async Task<IActionResult> GetCordsFromAddress([FromQuery(Name = "q")] string address)
        {
            var cords = await _mapsService.GetCordsFromAdress(address);

            return Ok(cords);
        }

        [HttpGet("route")]
        public async Task<IActionResult> GetRouteDistanceFromAddresses([FromQuery] string user, [FromQuery] string guincho)
        {
            var route = await _mapsService.GetRouteDistance(user, guincho);

            return Ok(route);
        }
    }
}
