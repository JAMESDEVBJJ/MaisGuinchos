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

        public async Task<User> GetUserById(Guid id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);

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
                   Lon = u.Lon
               },
               Stars = u.Stars,
               Model = u.Model,
               Color = u.Color,
               Available = u.Available,
               DistanceKm = GeoHelper.CalcularDistanciaKm(
                   userLocation.Latitude, userLocation.Longitude, 
                   u.Lat, u.Lon),
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
                    Model = u.Guincho!.Modelo
                })
                .ToListAsync();
        }

        public async Task<User> AddUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }

        public async Task Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
