

using MaisGuinchos.Models;

namespace MaisGuinchos.Repositorys.Interfaces
{
    public interface ITowTravelRepo
    {
        Task AddAsync(Models.TowTravel towTravel);

        Task<int> GetTotalCountByUserId(Guid userId);

        Task<List<TowTravel>> GetAllByUserId(Guid userId);

        Task<List<TowTravel>> GetAllByUserIdPaginated(Guid userId, int page, int pageSize);

        Task<TowTravel?> GetLastActiveByDriverId(Guid driverId);
        Task<TowTravel?> GetActiveByClientId(Guid clientId);

        Task<TowTravel?> GetPendingByUserId(Guid userId);

        Task SaveChangesAsync();
    }
}
