using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MaisGuinchos.Models;
using MaisGuinchos.Services;
using MaisGuinchos.Services.Interfaces;

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
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                var userAdd = await _userService.AddUser(user);

                if (userAdd == null)
                {
                    return BadRequest("Não foi possivel adicionar o usuário");
                }

                return CreatedAtAction(nameof(GetUserById), new {id = userAdd.Id}, userAdd);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}
