using MaisGuinchos.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaisGuinchos.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TowTravelController: ControllerBase
    {
        private readonly ITravelService _towTravelService;

        public TowTravelController(ITravelService travelService)
        {
            _towTravelService = travelService;
        }

        [Route("pending")]
        [HttpGet]
        public async Task<IActionResult> GetPendingTowTravel()
        {
            var user = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(user) || !Guid.TryParse(user, out var userId))
            {
                return Unauthorized("Usuário logado não encontrado.");
            }

            var pendingTowTravel = await _towTravelService.GetPendingTowTravel(userId);
            return Ok(pendingTowTravel);
        }

        [Route("{id}/start-journey")]
        [HttpPost]
        public async Task<IActionResult> StartJourney(Guid id)
        {
            var user = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(user) || !Guid.TryParse(user, out var userId))
            {
                return Unauthorized("Usuário logado não encontrado.");
            }

            var result = await _towTravelService.StartJourney(userId, id);
            return Ok(result);
        }
    }
}
