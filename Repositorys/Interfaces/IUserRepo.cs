using MaisGuinchos.Dtos;
using MaisGuinchos.Dtos.User;
using MaisGuinchos.Models;

namespace MaisGuinchos.Repositorys.Interfaces
{
    public interface IUserRepo
    {
        public List<User> GetAllUsers();

        public Task<User> GetUserByEmail(string email);

        public Task<User> GetUserByCpf(string cpf);

        public Task<User> GetUserById(Guid id);

        public Task<List<MotoristaProxDTO>> GetMotoristasProximos(Location userLocation);

        public Task<User> AddUser(User user);
        public Task Save();
    }
}
