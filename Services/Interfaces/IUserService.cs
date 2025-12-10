using MaisGuinchos.Models;

namespace MaisGuinchos.Services.Interfaces
{
    public interface IUserService
    {
        public List<User> GetAllUsers();

        public Task<User> GetUserById(int id); 

        public Task<User> AddUser(User user);
    }
}
