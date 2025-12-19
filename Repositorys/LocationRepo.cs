using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Dtos;

namespace MaisGuinchos.Repositorys
{
    public class LocationRepo : ILocationRepo
    {
        private readonly AppDbContext _dbContext;    

        public LocationRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Location> UpdateLocation(int userId, CreateLocationDTO location)
        {
            var locationAdd = new Location
            {
                Latitude = location.lat ?? 0,
                Longitude = location.lon ?? 0,
                DisplayName = location.display_name ?? "",
                UserId = userId
            };

            _dbContext.Locations.Add(locationAdd);

            await _dbContext.SaveChangesAsync();

            return locationAdd;
        }
    }
}
