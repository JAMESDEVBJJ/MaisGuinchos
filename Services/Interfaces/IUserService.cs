using MaisGuinchos.Dtos;
using MaisGuinchos.Models;

namespace MaisGuinchos.Services.Interfaces
{
    public interface IUserService
    {
        public List<User> GetAllUsers();

        public Task<User> GetUserById(Guid id); 

        public Task<User> AddUser(User user);

        public Task<LoginResponseDTO> LoginUser(UserLoginDTO user);

        public Task<User> UpdateUser(UpdUserDto userUpd, Guid id);

        public Task<UpdLocationResponseDTO> UpdateLocation(Guid id, AddressDTO address);
    }
}
