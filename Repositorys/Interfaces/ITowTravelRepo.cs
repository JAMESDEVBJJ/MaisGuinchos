

using MaisGuinchos.Models;

namespace MaisGuinchos.Repositorys.Interfaces
{
    public interface ITowTravelRepo
    {
        Task AddAsync(Models.TowTravel towTravel);
        Task<List<TowTravel>> GetAllByUserId(Guid userId);
        Task<TowTravel?> GetLastActiveByDriverId(Guid driverId);
        Task<TowTravel?> GetActiveByClientId(Guid clientId);

        Task<TowTravel?> GetPendingByUserId(Guid userId);

        Task SaveChangesAsync();
    }
}
