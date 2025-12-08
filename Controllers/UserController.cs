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
        public IActionResult GetUserById(int id) //ActionResult<User>
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            var userAdd = _userService.AddUser(user);

            if (userAdd == null)
            {
                return BadRequest("Não foi possivel adicionar o usuário");
            }

            return CreatedAtAction(userAdd.Name, userAdd);
        }

    }
}
