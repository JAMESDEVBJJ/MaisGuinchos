using MaisGuinchos.Dtos;
using MaisGuinchos.Dtos.Route;
using MaisGuinchos.Services;
using MaisGuinchos.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace MaisGuinchos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var addressDto = new AddressDTO
            {
                address = address
            };

            var cords = await _mapsService.GetCordsFromAddress(addressDto);

            return Ok(cords);
        }

        [HttpGet("route/distance")]
        public async Task<IActionResult> GetRouteDistanceFromAddresses([FromQuery] string user, [FromQuery] string guincho)
        {
            var route = await _mapsService.GetRouteDistance(user, guincho);

            return Ok(route);
        }

        [HttpPost("route/calculate")]
        public async Task<IActionResult> CalculateRoute([FromBody] CalculateRouteDTO routeDto)
        {
            if (string.IsNullOrEmpty(routeDto.Destination))
                return BadRequest("Destino inválido");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var destinationAddress = new AddressDTO
            {
                address = routeDto.Destination,
                id = null
            };

            var destinoCords = await _mapsService.GetCordsFromAddress(destinationAddress);

            if (destinoCords == null)
                return BadRequest("Não foi possível encontrar o destino");

            var destino = destinoCords.FirstOrDefault();

            if (destino == null)
                return BadRequest("Destino não encontrado");

            if (!double.TryParse(destino.lat, NumberStyles.Any, CultureInfo.InvariantCulture, out double destLat) ||
     !double.TryParse(destino.lon, NumberStyles.Any, CultureInfo.InvariantCulture, out double destLon))
            {
                return BadRequest("Coordenadas inválidas");
            }

            var route = await _mapsService.GetRoute(
                routeDto.OriginLat,
                routeDto.OriginLon,
                destLat,
                destLon
            );

            if (route == null)
                return BadRequest("Erro ao calcular rota");

            return Ok(route);
        }

        [HttpPost("route/calculate/driver")]
        public async Task<IActionResult> CalculateRouteDriver([FromBody] CalculateRouteDTO routeDto)
        {
            if (routeDto.DriverLat == null || routeDto.DriverLon == null)
                return BadRequest("Cordenadas do motorista inválidas.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var route = await _mapsService.GetRoute(
                routeDto.OriginLat,
                routeDto.OriginLon,
                routeDto.DriverLat,
                routeDto.DriverLon
            );

            if (route == null)
                return BadRequest("Erro ao calcular rota.");

            route.PriceEstimate = route.PriceEstimate / 2;

            return Ok(route);
        }

        [Authorize(Roles = "Motorista,Cliente")]
        [HttpGet("last-location")]
        public async Task<IActionResult> GetLastLocation()
        {
            var sub = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
          ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(sub)) return Unauthorized();

            var userId = Guid.Parse(sub);

            var lastLocation = await _mapsService.GetLastLocationAsync(userId);

            if (lastLocation == null)
            {
                return NotFound("Nenhuma localização encontrada para o usuário.");
            }

            return Ok(lastLocation);
        }
    }
}
