using MaisGuinchos.Dtos.Tow;
using MaisGuinchos.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MaisGuinchos.Controllers
{
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
        [Authorize(Roles = "Cliente")]
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
                return NotFound("Nenhum pedido de reboque encontrado.");
            return Ok(towRequest);
        }

        [HttpGet("pendings")]
        [Authorize(Roles = "Motorista")]
        public async Task<IActionResult> GetPendingTowRequests()
        {
            var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(idClaim) || !Guid.TryParse(idClaim, out var driverId))
            {
                return Unauthorized();
            }

            var pendingRequests = await _towRequestService.GetTowsPendings(driverId);

            return Ok(pendingRequests);
        }

        [HttpPut("{id}/counter-offer")]
        [Authorize(Roles = "Motorista")]
        public async Task<IActionResult> TowRequestCounterOffer(Guid id, [FromBody] TowRequestCounterOfferDto counterOffer)
        {
            if (counterOffer == null)
                return BadRequest("Dados inválidos");

            var result = await _towRequestService.UpdateTowRequestCounterOffer(id, counterOffer);

            return Ok(result);
        }

        [HttpPut("{id}/reject-counter-offer")]
        [Authorize(Roles = "Cliente")]

        public async Task<IActionResult> RejectCounterOffer(Guid id)
        {
            var result = await _towRequestService.RejectCounterOffer(id);

            return Ok(result);
        }
    }
}
