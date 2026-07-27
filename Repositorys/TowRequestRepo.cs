using MaisGuinchos.Dtos;
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

        public async Task<bool> HasActiveRequestAsync(Guid clientId, Guid driverId)
        {
            return await _appDbContext.TowRequests.AnyAsync(tr =>
                tr.ClientId == clientId &&
                tr.DriverId == driverId &&
                (tr.Status == TowRequestStatus.WaitingDriverResponse ||
                tr.Status == TowRequestStatus.CounterOfferSent));
        }

        public async Task<TowRequest?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.TowRequests
                .Include(x => x.Client)
                .Include(x => x.Driver).ThenInclude(d => d.Guincho)
                .FirstOrDefaultAsync(tr => tr.Id == id);
        }

        public async Task<TowRequest?> GetByIdIncludeLocations(Guid idTowRequest)
        {
            return await _appDbContext.TowRequests
                .Include(x => x.Client)
                .Include(x => x.Driver)
                .ThenInclude(x => x.Locations).FirstOrDefaultAsync(tr => tr.Id == idTowRequest);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<PaginatedResponse<TowRequest>> GetTowsRequestsByUserId(Guid userId, int page, int pageSize)
        {
            var query = _appDbContext.TowRequests
                .Where(tr => tr.ClientId == userId || tr.DriverId == userId);

            var totalItems = await query.CountAsync();

            var tows = await query
                .Include(x => x.Driver)
                .Include(x => x.Client)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<TowRequest>
            {
                Items = tows,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public async Task<List<TowRequest>> GetPendingsAsync(Guid driverId)
        {
            return await _appDbContext.TowRequests
                .Where(tr => tr.DriverId == driverId
                    && (tr.Status == TowRequestStatus.WaitingDriverResponse ||
                    tr.Status == TowRequestStatus.CounterOfferSent ||
                    tr.Status == TowRequestStatus.CounterOfferRejected) )
                .Where(tr => tr.CreatedAt == _appDbContext.TowRequests
                    .Where(x => x.ClientId == tr.ClientId &&
                                x.DriverId == tr.DriverId)
                    .Max(x => x.CreatedAt))
                .Include(x => x.Client)
                .ToListAsync();
        }

        public async Task<List<TowRequest>> GetClientPendingsAsync(Guid clientId)
        {
            var ids = await _appDbContext.TowRequests
                .Where(x => x.ClientId == clientId)
                .GroupBy(x => x.DriverId)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).First().Id)
                .ToListAsync();

            var requests = await _appDbContext.TowRequests
                .Where(x => ids.Contains(x.Id))
                .Include(x => x.Client)
                .Include(x => x.Driver)
                .Where(x => x.Status != TowRequestStatus.Cancelled &&
                            x.Status != TowRequestStatus.Accepted)
                .ToListAsync();

            return requests;
        }
        public async Task<TowRequest> UpdateAsync(TowRequest towRequest)
        {
            _appDbContext.TowRequests.Update(towRequest);
            await _appDbContext.SaveChangesAsync();

            return towRequest;
        }
    }
}
