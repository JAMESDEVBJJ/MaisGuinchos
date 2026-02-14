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


namespace MaisGuinchos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
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
        public async Task<IActionResult> GetAllMotoritasProx(int? limit = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("Id do usuário não encontrado.");
            }

            var motoristas = await _userService.BuscarMotoristasProximos(userId, limit);

            if (!motoristas.Any())
            {
                return BadRequest(new { message = "Usuário sem localização válida." });
            }

            return Ok(motoristas);
        }


        [HttpGet("")]
        public async Task<IActionResult> GetUserById([FromBody] Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdUserDto userUpd, [FromRoute] Guid id)
        {
            var updatedUser = await _userService.UpdateUser(userUpd, id);

            return Ok(updatedUser);
        }

        [HttpPost("location")]
        [Authorize(Roles = "Cliente,Motorista")]
        public async Task<IActionResult> UpdateLocation([FromBody] AddressDTO address)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("Usuário do user logado não encontrado.");
            }

            var userGuid = Guid.Parse(userId);

            var updatedLocation = await _userService.UpdateLocation(userGuid, address);

            return Ok(updatedLocation);
        }
    }
}
