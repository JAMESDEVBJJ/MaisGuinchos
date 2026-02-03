using MaisGuinchos.Dtos;
using MaisGuinchos.Dtos.User;
using MaisGuinchos.Models;

namespace MaisGuinchos.Services.Interfaces
{
    public interface IUserService
    {
        public List<User> GetAllUsers();

        public Task<User> GetUserById(Guid id); 

        public Task<UserAddedDTO> AddUser(CreateUserDTO user);

        public Task<LoginResponseDTO> LoginUser(UserLoginDTO user);

        public Task<User> UpdateUser(UpdUserDto userUpd, Guid id);

        public Task<UpdLocationResponseDTO> UpdateLocation(Guid id, AddressDTO address);

        public Task<List<MotoristaProxDTO?>> BuscarMotoristasProximos(string userId, int? limit = null);
    }
}
