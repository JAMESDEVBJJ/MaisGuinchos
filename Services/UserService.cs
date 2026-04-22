using MaisGuinchos.Dtos;
using MaisGuinchos.Dtos.Route;
using MaisGuinchos.Dtos.User;
using MaisGuinchos.Exceptions;
using MaisGuinchos.Hubs;
using MaisGuinchos.Models;
using MaisGuinchos.Repositorys;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services.Interfaces;
using MaisGuinchos.utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;

namespace MaisGuinchos.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapsService _mapsService;
        private readonly ILocationRepo _locationRepo;
        private readonly IJwtService _jwtService;
        private readonly ITravelService _travelService;
        private readonly IHubContext<TowHub> _hubContext;

        private readonly PasswordHasher _hasherUtil = new PasswordHasher();

        public UserService(IUserRepo userRepo,
            IMapsService mapsService,
            ILocationRepo locationRepo,
            IJwtService jwtService,
            ITravelService travelService,
            IHubContext<TowHub> hubContext)
        {
            _userRepo = userRepo;
            _mapsService = mapsService;
            _locationRepo = locationRepo;
            _jwtService = jwtService;
            _hubContext = hubContext;
            _travelService = travelService;
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
                    if (user.Guincho.Foto == null)
                        throw new ValidationException("Foto do guincho é obrigatória.");

                    var photoUrl = await SavePhotoAsync(user.Guincho.Foto);

                    userAdd.Guincho = new Guincho
                    {
                        Modelo = user.Guincho.Modelo,
                        Cor = user.Guincho.Cor,
                        Disponivel = true,
                        Placa = user.Guincho.Placa,
                        Foto = photoUrl
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
                    Name = userAdded.Name,
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

        private async Task<string> SavePhotoAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads"
            );

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
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

        public async Task<UpdLocationResponseDTO> UpdateLocation(Guid id, AddressDTO address, ClaimsPrincipal userClaims)
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

            var userLocationAdd = new CreateLocationDTO
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

            var locationAdded = await _locationRepo.UpdateLocation(id, userLocationAdd);

            var locationReturn = new UpdLocationResponseDTO
            {
                Lat = locationAdded.Latitude,
                Lon = locationAdded.Longitude,
                DisplayName = locationAdded.DisplayName,
                User = new UserSummaryDTO
                {
                    Id = id,
                    UserName = locationAdded.User.UserName
                }
            };

            var role = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role == "Motorista")
            {
                await HandleDriverLocationUpdate(id, locationAdded);
            }

            return locationReturn;
        }

        private async Task HandleDriverLocationUpdate(Guid driverId, Location updatedLocation)
        {
            if (updatedLocation?.Latitude == null || updatedLocation?.Longitude == null)
                return;

            var travel = await _travelService.GetActiveByDriverId(driverId);

            if (travel == null)
                return;

            var target = _travelService.ResolveTarget(travel);

            var route = await _mapsService.GetRoute(
                updatedLocation.Latitude,
                updatedLocation.Longitude,
                target.Lat,
                target.Lon
            );

            if (route == null)
                return;

            var realtime = new RouteRealtimeDTO
            {
                Type = travel.Status == TowTravelStatus.GoingToClient ?
                RouteType.DriverToPickup : RouteType.DriverToDestination,
                Origin = new CoordinateDto
                {
                    Lat = updatedLocation.Latitude,
                    Lon = updatedLocation.Longitude
                },
                Destination = new CoordinateDto
                {
                    Lat = target.Lat,
                    Lon = target.Lon
                },
                Polyline = route.Polyline,
                DistanceKm = route.DistanceKm//falta preço e minutes
            };

            await SendRouteUpdate(travel, realtime);
        }

        private async Task SendRouteUpdate(TowTravel travel, RouteRealtimeDTO route)
        {
            await _hubContext.Clients.User(travel.DriverId.ToString())
                .SendAsync("driverLocationUpdated", route);

            await _hubContext.Clients.User(travel.TowRequest.ClientId.ToString())
                .SendAsync("driverLocationUpdated", route);
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
