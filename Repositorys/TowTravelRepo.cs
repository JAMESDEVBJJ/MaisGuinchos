using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;

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
    }
}
