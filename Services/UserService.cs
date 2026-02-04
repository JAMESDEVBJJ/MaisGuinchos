using MaisGuinchos.Dtos;
using MaisGuinchos.Dtos.User;
using MaisGuinchos.Exceptions;
using MaisGuinchos.Models;
using MaisGuinchos.Repositorys;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services.Interfaces;
using MaisGuinchos.utils;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;

namespace MaisGuinchos.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapsService _mapsService;
        private readonly ILocationRepo _locationRepo;
        private readonly IJwtService _jwtService;

        private readonly PasswordHasher _hasherUtil = new PasswordHasher();

        public UserService(IUserRepo userRepo, IMapsService mapsService, ILocationRepo locationRepo, IJwtService jwtService)
        {
            _userRepo = userRepo;
            _mapsService = mapsService;
            _locationRepo = locationRepo;
            _jwtService = jwtService;
        }

        public List<User> GetAllUsers()
        {
            var users = _userRepo.GetAllUsers();

            return users;
        }

        public async Task<User> GetUserById(Guid id)
        {
            var user = await _userRepo.GetUserById(id);

            if (user == null)
            {
                throw new NotFoundException("User");
            }

            return user;
        }

        public async Task<UserAddedDTO?> AddUser(CreateUserDTO user)
        {
            var userExists = await _userRepo.GetUserByEmail(user.Email);

            if (userExists != null)
            {
                throw new Exception("User with this email already exist.");
            }

            if (!Enum.IsDefined(typeof(User.UserType), user.Tipo))
            {
                throw new ValidationException("User type invalid.");
            }

            var hash = _hasherUtil.Hasher(user.Password);

            var userAdd = new User
            {
                Name = user.Name,
                UserName = user.UserName,
                Cpf = user.Cpf,
                NumeroTelefone = user.NumeroTelefone,
                Estrelas = 5,
                Email = user.Email,
                Password = hash,
                Tipo = (User.UserType)user.Tipo
            };

            if (user.Tipo == 1)
            {
                if (user.Guincho != null)
                {
                    userAdd.Guincho = new Guincho
                    {
                        Modelo = user.Guincho.Modelo,
                        Cor = user.Guincho.Cor,
                        Disponivel = true,
                        Placa = user.Guincho.Placa
                    };
                } else
                {
                    throw new Exception("Guincho obrigatório para motorista.");
                } 
            }

            var userAdded = await _userRepo.AddUser(userAdd);

            if (userAdded != null)
            {
                var userDTO = new UserAddedDTO
                {
                    UserName = userAdded.UserName,
                    Name = userAdded.UserName,
                    Email = userAdded.Email,
                    NumeroTelefone = userAdded.NumeroTelefone,
                    Tipo = (UserAddedDTO.UserType)userAdded.Tipo
                };

                if(userAdded.Guincho != null)
                {
                    userDTO.Guincho = new CreateGuinchoRequest
                    {
                        Cor = userAdded.Guincho.Cor,
                        Modelo = userAdded.Guincho.Modelo,
                        Placa = userAdded.Guincho.Placa
                    };
                }

                return userDTO;
            }

            return null;
        }

        public async Task<LoginResponseDTO> LoginUser(UserLoginDTO userDto)
        {
            var user = await _userRepo.GetUserByEmail(userDto.Email);

            if (user == null)
            {
                throw new UnauthorizedException("Email not registered.");
            }

            var passwordValid = _hasherUtil.Verify(userDto.Password, user.Password);

            if ((!passwordValid && 0 != 0))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var token = _jwtService.GenerateToken(user);

            return new LoginResponseDTO
            {
                Token = token
            };
        }

        public async Task<User> UpdateUser(UpdUserDto userUpd, Guid id)
        {
            var user = await _userRepo.GetUserById(id);

            if (user == null)
            {
                throw new NotFoundException("User");
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

        public async Task<UpdLocationResponseDTO> UpdateLocation(Guid id, AddressDTO address)
        {
            var user = await _userRepo.GetUserById(id);

            if (user == null)
            {
                throw new NotFoundException("User");
            }

            var geoLocation = await _mapsService.GetCordsFromAddress(address);

            if (geoLocation == null || geoLocation.Count == 0)
            {
                throw new BadRequestException("Address not exists");
            }

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

        public async Task<List<MotoristaProxDTO?>> BuscarMotoristasProximos(string userId, int? limit = null)
        {
            if (!Guid.TryParse(userId, out var userGuid))
            {
                throw new ArgumentException("UserId invalid.");
            }

            var userLocation = await _locationRepo.GetLastFromUser(userGuid);

            if (userLocation == null)
            {
                return [];
            }

            var guinchosProximos = await _userRepo.GetMotoristasProximos(userLocation);

            return guinchosProximos!;
        }
    }
}
