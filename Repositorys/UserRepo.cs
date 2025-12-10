using MaisGuinchos.Repositorys.Interfaces;
  
using MaisGuinchos.Models;
using Microsoft.EntityFrameworkCore;

namespace MaisGuinchos.Repositorys
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _dbContext;

        public UserRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<User> GetAllUsers()
        {
            var users = _dbContext.Users.ToList();

            return users;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);

            return user;
        }   

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<User> AddUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }
    }
}
