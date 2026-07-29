using MaisGuinchos.Dtos;
using MaisGuinchos.Dtos.User;
using MaisGuinchos.Models;
using System.Security.Claims;

namespace MaisGuinchos.Services.Interfaces
{
    public interface IUserService
    {
        public List<User> GetAllUsers();

        public Task<User> GetUserById(Guid id);

        public Task<UserProfileResponseDTO> GetUserProfileById(Guid id);

        public Task<GetUserStatusResponse> GetUserStatus(Guid id);

        public Task<UserAddedDTO> AddUser(CreateUserDTO user);

        public Task<LoginResponseDTO> LoginUser(UserLoginDTO user);

        public Task<UpdateUserProfileResponseDTO> UpdateUserProfile(UpdateUserProfileDTO userUpd, Guid id);

        public Task<UpdLocationResponseDTO> UpdateLocation(Guid id, AddressDTO address, ClaimsPrincipal userClaims);

        public Task UpdatePassword(UpdatePasswordDTO dto, Guid userId);

        public Task<List<MotoristaProxDTO?>> BuscarMotoristasProximos(Guid userId, int? limit = null);

        public Task<MotoristaProxDTO?> GetMotoristaProxById(Guid userId, Guid id);
    }
}
