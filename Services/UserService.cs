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
        private readonly ITowTravelRepo _towTravelRepo;
        private readonly IHubContext<TowHub> _hubContext;

        private readonly PasswordHasher _hasherUtil = new PasswordHasher();

        private const double DISTANCE_TO_ARRIVED_METERS = 200;

        public UserService(IUserRepo userRepo,
            IMapsService mapsService,
            ILocationRepo locationRepo,
            IJwtService jwtService,
            ITravelService travelService,
            IHubContext<TowHub> hubContext, ITowTravelRepo towTravelRepo)
        {
            _userRepo = userRepo;
            _mapsService = mapsService;
            _locationRepo = locationRepo;
            _jwtService = jwtService;
            _towTravelRepo = towTravelRepo;
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

        public async Task<UserProfileResponseDTO> GetUserProfileById(Guid id)
        {
            var user = await _userRepo.GetUserById(id);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            if (user.Tipo == User.UserType.Motorista && user.Guincho != null)
            {
                return new UserProfileResponseDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Cpf = user.Cpf,
                    Name = user.Name,
                    Estrelas = user.Estrelas,
                    NumeroTelefone = user.NumeroTelefone,
                    Tipo = ((UserType)user.Tipo).ToString(),
                    UserName = user.UserName,
                    Guincho = new Dtos.Guincho.TowGuinchoDTO
                    {
                        Id = user.Guincho.Id,
                        Model = user.Guincho.Modelo,
                        Color = user.Guincho.Cor,
                        Plate = user.Guincho.Placa,
                        Photo = user.Guincho.Foto
                    }
                };
            }

            return new UserProfileResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                Cpf = user.Cpf,
                Name = user.Name,
                Estrelas = user.Estrelas,
                NumeroTelefone = user.NumeroTelefone,
                Tipo = ((UserType)user.Tipo).ToString(),
                UserName = user.UserName
            };
        }

        public async Task<GetUserStatusResponse> GetUserStatus(Guid id)
        {
            var user = await _userRepo.GetUserById(id);

            if (user == null)
            {
                throw new NotFoundException("User");
            }

            if (user.Tipo != User.UserType.Motorista || user.Guincho == null)
            {
                throw new BadRequestException("User is not a driver.");
            }

            return new GetUserStatusResponse
            {
                Status = user.Guincho.Disponivel
            };
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

                }
                else
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

                if (userAdded.Guincho != null)
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

        public async Task<UpdateUserProfileResponseDTO> UpdateUserProfile(UpdateUserProfileDTO userUpd, Guid id)
        {
            var user = await _userRepo.GetUserById(id);

            if (user == null)
            {
                throw new NotFoundException("User");
            }

            if (!string.IsNullOrWhiteSpace(userUpd.Name))
            {
                user.Name = userUpd.Name;
            }

            if (!string.IsNullOrWhiteSpace(userUpd.UserName))
            {
                var exist = await _userRepo.GetUserByUserName(userUpd.UserName);

                if (exist != null && exist.Id != user.Id)
                {
                    throw new BadRequestException(
                        "Este nome de usuário já está em uso."
                    );
                }

                user.UserName = userUpd.UserName;
            }

            if (!string.IsNullOrWhiteSpace(userUpd.NumeroTelefone))
            {
                user.NumeroTelefone = userUpd.NumeroTelefone;
            }

            if (!string.IsNullOrWhiteSpace(userUpd.Email) && userUpd.Email != user.Email)
            {
                var exist = await _userRepo.GetUserByEmail(userUpd.Email);

                if (exist != null)
                {
                    throw new Exception(
                        "User with this email already exist."
                    );
                }

                user.Email = userUpd.Email;
            }

            if (user.Tipo == User.UserType.Motorista)
            {
                if (userUpd.Photo != null)
                {
                    var photoUrl = await SavePhotoAsync(userUpd.Photo);
                    user.Guincho!.Foto = photoUrl;
                }
                if (userUpd.Guincho != null)
                {
                    user.Guincho!.Modelo = userUpd.Guincho.Model ?? user.Guincho.Modelo;
                    user.Guincho!.Cor = userUpd.Guincho.Color ?? user.Guincho.Cor;
                    user.Guincho!.Placa = userUpd.Guincho.Plate ?? user.Guincho.Placa;
                }
            }

            var userUpdated = await _userRepo.UpdateUser(user);

            return new UpdateUserProfileResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                NumeroTelefone = user.NumeroTelefone,
                Cpf = user.Cpf,
                Tipo = user.Tipo.ToString(),
                Guincho = user.Guincho != null ? new Dtos.Guincho.TowGuinchoDTO
                {
                    Id = user.Guincho.Id,
                    Model = user.Guincho.Modelo,
                    Color = user.Guincho.Cor,
                    Plate = user.Guincho.Placa,
                    Photo = user.Guincho.Foto
                } : null,
            };
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
                await HandleRealtimeTravelTracking(id, locationAdded);
            }

            return locationReturn;
        }

        public async Task UpdatePassword(UpdatePasswordDTO dto, Guid userId)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
            {
                throw new BadRequestException("A nova senha e a confirmação não coincidem.");
            }

            var user = await _userRepo.GetUserById(userId);

            if (user == null)
            {
                throw new NotFoundException("User");
            }

            var passwordValid = _hasherUtil.Verify(
                dto.CurrentPassword,
                user.Password
            );

            if (!passwordValid)
            {
                throw new BadRequestException("A senha atual está incorreta.");
            }

            if (_hasherUtil.Verify(dto.NewPassword, user.Password))
            {
                throw new BadRequestException(
                    "A nova senha deve ser diferente da senha atual."
                );
            }

            user.Password = _hasherUtil.Hasher(dto.NewPassword);

            await _userRepo.UpdateUser(user);
        }

        private async Task HandleRealtimeTravelTracking(Guid driverId, Location updatedLocation)
        {
            if (updatedLocation?.Latitude == null || updatedLocation?.Longitude == null)
                return;

            var travel = await _travelService.GetActiveByDriverId(driverId);

            if (travel != null &&
                (travel.Status == TowTravelStatus.InProgress || travel.Status == TowTravelStatus.GoingToClient))
            {
                var target = _travelService.ResolveTarget(travel);

                var route = await _mapsService.GetRoute(
                    updatedLocation.Latitude,
                    updatedLocation.Longitude,
                    target!.Lat,
                    target!.Lon
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
                    DistanceKm = route.DistanceKm,
                    DurationMinutes = route.DurationMinutes,
                };

                await SendRouteUpdate(travel, realtime);

                if (travel.Status == TowTravelStatus.GoingToClient)
                {
                    var distanceToPickupM = GeoHelper.CalcularDistanciaKm(updatedLocation.Latitude, updatedLocation.Longitude,
                        target.Lat, target.Lon) * 1000;
                    if (distanceToPickupM <= DISTANCE_TO_ARRIVED_METERS)
                    {
                        travel.Status = TowTravelStatus.ArrivedAtPickup;

                        await _hubContext.Clients.User(travel.TowRequest.ClientId.ToString())
                            .SendAsync("DriverArrivedAtPickup");
                        await _hubContext.Clients.User(travel.DriverId.ToString())
                            .SendAsync("ArrivedAtPickup");

                        await _towTravelRepo.SaveChangesAsync();
                    }
                }
                else if (travel.Status == TowTravelStatus.InProgress)
                {
                    var distanceToDropoffM = GeoHelper.CalcularDistanciaKm(updatedLocation.Latitude, updatedLocation.Longitude,
                        target.Lat, target.Lon) * 1000;
                    if (distanceToDropoffM <= DISTANCE_TO_ARRIVED_METERS)
                    {
                        travel.Status = TowTravelStatus.ArrivedAtDestination;

                        await _hubContext.Clients.User(travel.TowRequest.ClientId.ToString())
                            .SendAsync("DriverArrivedAtDestination");
                        await _hubContext.Clients.User(travel.DriverId.ToString())
                           .SendAsync("ArrivedAtDestination");

                        await _towTravelRepo.SaveChangesAsync();
                    }
                }
            }
        }

        private async Task SendRouteUpdate(TowTravel travel, RouteRealtimeDTO route)
        {
            await _hubContext.Clients.User(travel.DriverId.ToString())
                .SendAsync("DriverLocationUpdated", route);

            await _hubContext.Clients.User(travel.TowRequest.ClientId.ToString())
                .SendAsync("DriverLocationUpdated", route);
        }

        public async Task<List<MotoristaProxDTO?>> BuscarMotoristasProximos(Guid userId, int? limit = null)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("UserId invalid.");
            }

            var userLocation = await _locationRepo.GetLastFromUser(userId);

            if (userLocation == null)
            {
                return [];
            }

            var guinchosProximos = await _userRepo.GetMotoristasProximos(userLocation);

            return guinchosProximos!;
        }

        public async Task<MotoristaProxDTO?> GetMotoristaProxById(
            Guid userId,
            Guid id)
        {
            var userLocation = await _locationRepo.GetLastFromUser(userId);

            if (userLocation == null)
            {
                throw new NotFoundException("User location not found.");
            }

            var motorista = await _userRepo.GetMotoristaById(id, userLocation.Latitude, userLocation.Longitude);

            if (motorista == null)
            {
                throw new NotFoundException("Motorista not found.");
            }

            if (!motorista.Available)
            {
                throw new BusinessException("Motorista não está disponível.");
            }

            return motorista;
        }
    }
}
