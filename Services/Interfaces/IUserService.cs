using MaisGuinchos.Models;

namespace MaisGuinchos.Services.Interfaces
{
    public interface IUserService
    {
        public List<User> GetAllUsers();

        public User AddUser(User user);
    }
}
