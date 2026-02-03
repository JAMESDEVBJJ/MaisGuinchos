using MaisGuinchos.Services;
using MaisGuinchos.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
