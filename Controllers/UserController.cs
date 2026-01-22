using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MaisGuinchos.Models;
using MaisGuinchos.Services;
using MaisGuinchos.Services.Interfaces;
using MaisGuinchos.Dtos;
using Microsoft.AspNetCore.Routing.Constraints;
using Npgsql.Replication.PgOutput.Messages;
using Microsoft.AspNetCore.Authorization;

namespace MaisGuinchos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            List<User> users = _userService.GetAllUsers();

            if (users == null || users.Count == 0)
            {
                return NotFound("Nenhum usuário encontrado");
            }

            return Ok(users);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            var userAdd = await _userService.AddUser(user);

            if (userAdd == null)
            {
                return BadRequest("Não foi possivel adicionar o usuário");
            }

            return CreatedAtAction(nameof(GetUserById), new {id = userAdd.Id}, userAdd);
            
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

        [HttpPost("location/{id}")]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> UpdateLocation([FromBody] AddressDTO address, [FromRoute] Guid id)
        {
            var updatedLocation = await _userService.UpdateLocation(id, address);

            return Ok(updatedLocation);
        }
    }
}
