using MaisGuinchos.Dtos.Guincho;
using MaisGuinchos.Services;
using MaisGuinchos.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MaisGuinchos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuinchoController : ControllerBase
    {
        private readonly IGuinchoService _guinchoService;

        public GuinchoController(IGuinchoService guinchoService) {
            _guinchoService = guinchoService;
        }

        [HttpPut("status")]
        [Authorize(Roles = "Motorista")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateGuinchoStatusDTO status)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("Id do usuário não encontrado.");
            }

            var updatedGuincho = await _guinchoService.UpdateStatus(userId, status);

            if (updatedGuincho == null)
            {
                return NotFound("Guincho não encontrado para o usuário.");
            }

            return Ok(updatedGuincho);
        }

        /*public IActionResult GetGuinchos()
        {

        }

        public IActionResult CreateGuincho([FromBody] GTS guincho)
        {
            var guincho = _guinchoService.addGuincho(guincho);

            return Ok(guincho);
        }
        */
    }
}
