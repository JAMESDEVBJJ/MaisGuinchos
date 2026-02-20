using MaisGuinchos.Dtos.Tow;
using MaisGuinchos.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaisGuinchos.Controllers
{
    [Authorize(Roles = "Cliente")]
    [ApiController]
    [Route("api/[controller]")]
    public class TowRequestsController : ControllerBase
    {
        private readonly ITowRequestService _towRequestService;

        public TowRequestsController(ITowRequestService towRequestService)
        {
            _towRequestService = towRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTowRequest([FromBody] CreateTowRequestDto dto)
        {
            var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value; 

            if (string.IsNullOrWhiteSpace(idClaim) || !Guid.TryParse(idClaim, out var clientId))
            {
                return Unauthorized();
            }

            var towRequestId = await _towRequestService.CreateAsync(clientId, dto);

            return Ok(new { TowRequestId = towRequestId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTowRequestById(Guid id)
        {
            var towRequest = await _towRequestService.GetTowRequestById(id);
            if (towRequest == null)
                return NotFound();
            return Ok(towRequest);
        }
    }
}
