using MaisGuinchos.Models;
using MaisGuinchos.Repositorys;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services.Interfaces;
using MaisGuinchos.utils;

namespace MaisGuinchos.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly PasswordHasher _hasherUtil = new PasswordHasher();

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public List<User> GetAllUsers()
        {
            var users = _userRepo.GetAllUsers();

            return users;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _userRepo.GetUserById(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return user;
        }

        public async Task<User> AddUser(User user)
        {
            var userExists = await _userRepo.GetUserByEmail(user.Email);

            if (userExists != null)
            {
                throw new Exception("User with this email already exist.");
            }
            
            var hash = _hasherUtil.Hasher(user.Password);
            user.Password = hash;

            var userAdd = await _userRepo.AddUser(user);

            return userAdd;  
        }
    }
}
