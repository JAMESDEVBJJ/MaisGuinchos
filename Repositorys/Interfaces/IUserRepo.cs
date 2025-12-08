using MaisGuinchos.Models;

namespace MaisGuinchos.Repositorys.Interfaces
{
    public interface IUserRepo
    {
        public List<User> GetAllUsers();

        public User AddUser(User user);
    }
}
