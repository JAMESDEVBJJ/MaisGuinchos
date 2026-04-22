using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaisGuinchos.Repositorys
{
    public class TowTravelRepo : ITowTravelRepo
    {
        private readonly AppDbContext _appDbContext;
        public TowTravelRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddAsync(TowTravel towTravel)
        {
            await _appDbContext.TowTravels.AddAsync(towTravel);
            await _appDbContext.SaveChangesAsync(); 
        }

        public async Task<TowTravel?> GetActiveByDriverId(Guid driverId)
        {
            return await _appDbContext.TowTravels
                .Include(t => t.TowRequest)
                .FirstOrDefaultAsync(t => t.DriverId == driverId &&
                (t.Status != TowTravelStatus.Finished &&
                t.Status != TowTravelStatus.Cancelled));
        }

        public async Task<TowTravel?> GetActiveByClientId(Guid clientId)
        {
            return await _appDbContext.TowTravels
                .FirstOrDefaultAsync(t => t.TowRequest.ClientId == clientId &&
                (t.Status != TowTravelStatus.Finished &&
                t.Status != TowTravelStatus.Cancelled));
        }
    }
}
