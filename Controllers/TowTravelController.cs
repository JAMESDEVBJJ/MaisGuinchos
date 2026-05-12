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
                return Unauthorized();
            }

            var pendingTowTravel = await _towTravelService.GetPendingTowTravel(userId);
            return Ok(pendingTowTravel);
        }
    }
}
