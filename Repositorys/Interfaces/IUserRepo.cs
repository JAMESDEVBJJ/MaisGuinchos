using MaisGuinchos.Models;

namespace MaisGuinchos.Repositorys.Interfaces
{
    public interface IUserRepo
    {
        public List<User> GetAllUsers();

        public Task<User> GetUserByEmail(string email);

        public Task<User> GetUserById(int id);

        public Task<User> AddUser(User user);
    }
}
