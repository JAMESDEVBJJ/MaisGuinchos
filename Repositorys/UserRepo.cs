using MaisGuinchos.Repositorys.Interfaces;

using MaisGuinchos.Models;
using Microsoft.EntityFrameworkCore;
using MaisGuinchos.Dtos;
using MaisGuinchos.utils;

namespace MaisGuinchos.Repositorys
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _dbContext;
        private readonly ILocationRepo _locationRepo;

        public UserRepo(AppDbContext dbContext, ILocationRepo locationRepo)
        {
            _dbContext = dbContext;
            _locationRepo = locationRepo;
        }

        public List<User> GetAllUsers()
        {
            var users = _dbContext.Users.ToList();

            return users;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            var user = _dbContext.Users.Include(u => u.Guincho).FirstOrDefault(u => u.Id == id);

            return user;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<User?> GetUserByCpf(string cpf)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Cpf == cpf);

            return user;
        }

        public async Task<User?> GetUserByUserName(string userName)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            return user;
        }   

        public async Task<MotoristaProxDTO?> GetMotoristaById(
            Guid id,
            double userLat,
            double userLon)
        {
            var motorista = await _dbContext.Users
                .Where(u =>
                    u.Id == id &&
                    u.Tipo == User.UserType.Motorista
                )
                .Select(u => new
                {
                    UserId = u.Id,
                    Name = u.Name,

                    Location = u.Locations
                        .OrderByDescending(l => l.CreatedAt)
                        .Select(l => new
                        {
                            l.Latitude,
                            l.Longitude
                        })
                        .FirstOrDefault(),

                    Foto = u.Guincho!.Foto,
                    Placa = u.Guincho!.Placa,
                    Number = u.NumeroTelefone,

                    Stars = u.Estrelas,
                    Model = u.Guincho.Modelo,
                    Color = u.Guincho.Cor,
                    Available = u.Guincho.Disponivel
                })
                .FirstOrDefaultAsync();

            if (motorista == null || motorista.Location == null)
            {
                return null;
            }

            var distanceKm = GeoHelper.CalcularDistanciaKm(
                motorista.Location.Latitude,
                motorista.Location.Longitude,
                userLat,
                userLon
            );

            return new MotoristaProxDTO
            {
                Motorista = new MotoristaComLoc
                {
                    UserId = motorista.UserId,
                    Name = motorista.Name,

                    Lat = motorista.Location.Latitude,
                    Lon = motorista.Location.Longitude,

                    Foto = motorista.Foto,
                    Placa = motorista.Placa,
                    Number = motorista.Number
                },

                Stars = motorista.Stars,
                Model = motorista.Model,
                Color = motorista.Color,
                Available = motorista.Available,
                DistanceKm = distanceKm
            };
        }

        public async Task<List<MotoristaProxDTO>> GetMotoristasProximos(Location userLocation)
        {
            var users = await GetAllMotoristasComLoc();

            return users.Select(u => new MotoristaProxDTO
            {
                Motorista = new MotoristaComLoc
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Lat = u.Lat,
                    Lon = u.Lon,
                    Foto = u.Foto,
                    Placa = u.Placa,
                    Number = u.Number
                },
                Stars = u.Stars,
                Model = u.Model,
                Color = u.Color,
                Available = u.Available,
                DistanceKm = GeoHelper.CalcularDistanciaKm(
                   userLocation.Latitude, userLocation.Longitude,
                   u.Lat, u.Lon)
            }).OrderBy(m => m.DistanceKm).Take(10).ToList();
        }

        public async Task<List<MotoristaComLoc>> GetAllMotoristasComLoc()
        {
            return await _dbContext.Users
                .Where(u =>
                    u.Tipo == User.UserType.Motorista &&
                    u.Locations.Any()
                )
                .Select(u => new MotoristaComLoc
                {
                    UserId = u.Id,
                    Name = u.Name,
                    Lat = u.Locations
                        .OrderByDescending(l => l.CreatedAt)
                        .Select(l => l.Latitude)
                        .FirstOrDefault(),
                    Lon = u.Locations
                        .OrderByDescending(l => l.CreatedAt)
                        .Select(l => l.Longitude)
                        .FirstOrDefault(),
                    Available = u.Guincho!.Disponivel,
                    Stars = u.Estrelas,
                    Color = u.Guincho!.Cor,
                    Model = u.Guincho!.Modelo,
                    Foto = u.Guincho!.Foto,
                    Placa = u.Guincho.Placa,
                    Number = u.NumeroTelefone
                })
                .ToListAsync();
        }

        public async Task<User> AddUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
            return user;
        }

        public async Task Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
