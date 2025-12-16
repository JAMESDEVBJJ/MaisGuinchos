using MaisGuinchos.Dtos;
using MaisGuinchos.Models;
using MaisGuinchos.Repositorys;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services.Interfaces;
using MaisGuinchos.utils;
using System.Data;

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

        public async Task<User> UpdateUser(UpdUserDto userUpd, int id)
        {
            var user = await _userRepo.GetUserById(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Name = userUpd.Name ?? user.Name;
            user.UserName = userUpd.UserName ?? user.UserName;

            if (userUpd.Cpf != null && userUpd.Cpf != user.Cpf)
            {
                var exist  = await _userRepo.GetUserByCpf(userUpd.Cpf);

                if (exist != null)
                {
                    throw new Exception("User with this CPF already exist.");
                }

                user.Cpf = userUpd.Cpf;
            }
            
            user.NumeroTelefone = userUpd.NumeroTelefone ?? user.NumeroTelefone;
            user.Password = userUpd.Password != null ? _hasherUtil.Hasher(userUpd.Password) : user.Password;

            if (userUpd.Email != null && userUpd.Email != user.Email)
            {
                var exist = await _userRepo.GetUserByEmail(userUpd.Email);

                if (exist != null)
                {
                    throw new Exception("User with this email already exist.");

                }

                user.Email = userUpd.Email;
            }

            await _userRepo.Save();

            return await _userRepo.GetUserById(id);
        }
    }
}
