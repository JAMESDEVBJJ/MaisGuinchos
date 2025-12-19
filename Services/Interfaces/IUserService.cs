using MaisGuinchos.Dtos;
using MaisGuinchos.Models;

namespace MaisGuinchos.Services.Interfaces
{
    public interface IUserService
    {
        public List<User> GetAllUsers();

        public Task<User> GetUserById(int id); 

        public Task<User> AddUser(User user);

        public Task<User> UpdateUser(UpdUserDto userUpd, int id);

        public Task<UpdLocationResponseDTO> UpdateLocation(int id, AddressDTO address);
    }
}
