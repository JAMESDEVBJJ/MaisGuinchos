using MaisGuinchos.Models;
using MaisGuinchos.Repositorys;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services.Interfaces;

namespace MaisGuinchos.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public List<User> GetAllUsers()
        {
            var users = _userRepo.GetAllUsers();

            return users;
        }

        public User AddUser(User user)
        {
            var userAdd = _userRepo.AddUser(user);

            return userAdd;  
        }
    }
}
