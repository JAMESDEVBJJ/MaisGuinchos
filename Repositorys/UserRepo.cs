using MaisGuinchos.Repositorys.Interfaces;
  
using MaisGuinchos.Models;

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

        public User AddUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }
    }
}
