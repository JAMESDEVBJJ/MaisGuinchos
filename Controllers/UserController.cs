using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MaisGuinchos.Models;
using MaisGuinchos.Services;
using MaisGuinchos.Services.Interfaces;
using MaisGuinchos.Dtos;
using MaisGuinchos.Dtos.User;
using Microsoft.AspNetCore.Routing.Constraints;
using Npgsql.Replication.PgOutput.Messages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MaisGuinchos.Dtos.Route;


namespace MaisGuinchos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITravelService _travelService;
        private readonly IMapsService _mapsService;
        public UserController(IUserService userService, ITravelService travelService, IMapsService mapsService)
        {
            _userService = userService;
            _travelService = travelService;
            _mapsService = mapsService;
        }

        [HttpGet("all")]
        public IActionResult GetUsers()
        {
            List<User> users = _userService.GetAllUsers();

            if (users == null || users.Count == 0)
            {
                return NotFound("Nenhum usuário encontrado");
            }

            return Ok(users);
        }
        [HttpGet("proximos")]
        [Authorize]
        public async Task<IActionResult> GetAllMotoritasProx(
            int? limit = null,
            string? filtros = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("Id do usuário não encontrado.");
            }

            var filtrosAtivos = FiltroOrdenacaoParser.Parse(filtros);

            var motoristas = await _userService.BuscarMotoristasProximos(
                Guid.Parse(userId), limit, filtrosAtivos);

            if (!motoristas.Any())
            {
                return BadRequest(new { message = "Usuário sem localização válida." });
            }

            return Ok(motoristas);
        }

        [Authorize]
        [HttpGet("driver/{driverId:guid}")]
        public async Task<IActionResult> GetGuinchoByDriverId(Guid driverId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("Id do usuário não encontrado.");
            }

            var guincho = await _userService.GetMotoristaProxById(Guid.Parse(userId), driverId);

            if (guincho == null)
            {
                return NotFound("Não foi possível encontrar o motorista.");
            }

            return Ok(guincho);
        }


        [HttpGet("")]
        public async Task<IActionResult> GetUserById([FromBody] Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("Id do usuário não encontrado.");
            }

            var user = await _userService.GetUserProfileById(Guid.Parse(userId));

            if (user == null) return NotFound("Usuário não encontrado.");

            return Ok(user);
        }

        [Authorize]
        [HttpGet("status")]
        public async Task<IActionResult> GetUserStatus()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("Id do usuário não encontrado.");
            }

            var status = await _userService.GetUserStatus(Guid.Parse(userId));

            return Ok(status);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddUser([FromForm] CreateUserDTO user)
        {
            var userAdd = await _userService.AddUser(user);

            if (userAdd == null)
            {
                return BadRequest("Não foi possivel adicionar o usuário");
            }

            return CreatedAtAction(nameof(GetUserById), userAdd);

        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _userService.LoginUser(userLogin);

            return Ok(token);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserProfileDTO userUpd)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim);

            var updatedUser = await _userService.UpdateUserProfile(userUpd, userId);

            return Ok(updatedUser);
        }

        [HttpPost("location")]
        [Authorize(Roles = "Cliente,Motorista")]
        public async Task<IActionResult> UpdateLocation([FromBody] AddressDTO address)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("Usuário logado não encontrado.");
            }

            var userGuid = Guid.Parse(userId);

            var updatedLocation = await _userService.UpdateLocation(userGuid, address, User);

            return Ok(updatedLocation);
        }

        [Authorize]
        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            await _userService.UpdatePassword(dto, Guid.Parse(userIdClaim));

            return Ok(new
            {
                message = "Senha atualizada com sucesso."
            });
        }
    }
}
