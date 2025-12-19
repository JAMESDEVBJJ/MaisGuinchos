using MaisGuinchos.Dtos;
using MaisGuinchos.Models;
using MaisGuinchos.Repositorys;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services.Interfaces;
using MaisGuinchos.utils;
using System.Data;
using System.Globalization;

namespace MaisGuinchos.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapsService _mapsService;
        private readonly ILocationRepo _locationRepo;

        private readonly PasswordHasher _hasherUtil = new PasswordHasher();

        public UserService(IUserRepo userRepo, IMapsService mapsService, ILocationRepo locationRepo)
        {
            _userRepo = userRepo;
            _mapsService = mapsService;
            _locationRepo = locationRepo;
        }

        public List<User> GetAllUsers()
        {
            var users = _userRepo.GetAllUsers();

            return users;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _userRepo.GetUserById(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return user;
        }

        public async Task<User> AddUser(User user)
        {
            var userExists = await _userRepo.GetUserByEmail(user.Email);

            if (userExists != null)
            {
                throw new Exception("User with this email already exist.");
            }

            var hash = _hasherUtil.Hasher(user.Password);
            user.Password = hash;

            var userAdd = await _userRepo.AddUser(user);

            return userAdd;
        }

        public async Task<User> UpdateUser(UpdUserDto userUpd, int id)
        {
            var user = await _userRepo.GetUserById(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Name = userUpd.Name ?? user.Name;
            user.UserName = userUpd.UserName ?? user.UserName;

            if (userUpd.Cpf != null && userUpd.Cpf != user.Cpf)
            {
                var exist = await _userRepo.GetUserByCpf(userUpd.Cpf);

                if (exist != null)
                {
                    throw new Exception("User with this CPF already exist.");
                }

                user.Cpf = userUpd.Cpf;
            }

            user.NumeroTelefone = userUpd.NumeroTelefone ?? user.NumeroTelefone;
            user.Password = userUpd.Password != null ? _hasherUtil.Hasher(userUpd.Password) : user.Password;

            if (userUpd.Email != null && userUpd.Email != user.Email)
            {
                var exist = await _userRepo.GetUserByEmail(userUpd.Email);

                if (exist != null)
                {
                    throw new Exception("User with this email already exist.");

                }

                user.Email = userUpd.Email;
            }

            await _userRepo.Save();

            return await _userRepo.GetUserById(id);
        }

        public async Task<UpdLocationResponseDTO> UpdateLocation(int id, AddressDTO address)
        {
            var user = await _userRepo.GetUserById(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var geoLocation = await _mapsService.GetCordsFromAddress(address);

            var userLocation = new CreateLocationDTO
            {
                lat = double.TryParse(
                    geoLocation?[0].lat,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var lat
                    ) ? lat : 0,
                lon = double.TryParse(
                    geoLocation?[0].lon,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var lon
                    ) ? lon : 0,
                display_name = geoLocation != null & geoLocation?.Count > 0 ? geoLocation?[0].display_name : "",
            };

            var locationAdd = await _locationRepo.UpdateLocation(id, userLocation);

            var locationReturn = new UpdLocationResponseDTO
            {
                Lat = locationAdd.Latitude,
                Lon = locationAdd.Longitude,
                DisplayName = locationAdd.DisplayName,
                User = new UserSummaryDTO
                {
                    Id = id,
                    UserName = locationAdd.User.UserName
                }
            };

            return locationReturn;
        }
    }
}
