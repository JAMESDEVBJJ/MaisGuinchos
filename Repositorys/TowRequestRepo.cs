using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaisGuinchos.Repositorys
{
    public class TowRequestRepo : ITowRequestRepo
    {
        private readonly AppDbContext _appDbContext;
        public TowRequestRepo(AppDbContext appDbContext) {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(TowRequest request)
        {
            await _appDbContext.TowRequests.AddAsync(request);
        }

        public async Task<TowRequest?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.TowRequests
                .Include(x => x.Client)
                .Include(x => x.Driver).FirstOrDefaultAsync(tr => tr.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<TowRequest>> GetPendingsAsync(Guid driverId)
        {
            return await _appDbContext.TowRequests
                .Where(tr => tr.DriverId == driverId
                    && tr.Status == TowRequestStatus.WaitingDriverResponse)
                .Where(tr => tr.CreatedAt == _appDbContext.TowRequests
                    .Where(x => x.ClientId == tr.ClientId)
                    .Max(x => x.CreatedAt))
                .Include(x => x.Client)
                .ToListAsync();
        }

        public async Task<TowRequest> UpdateCounterOfferAsync(TowRequest towRequest)
        {
            _appDbContext.TowRequests.Update(towRequest);
            await _appDbContext.SaveChangesAsync();

            return towRequest;
        }
    }
}
