

using MaisGuinchos.Models;

namespace MaisGuinchos.Repositorys.Interfaces
{
    public interface ITowTravelRepo
    {
        Task AddAsync(Models.TowTravel towTravel);
        Task<TowTravel?> GetActiveByDriverId(Guid driverId);
        Task<TowTravel?> GetActiveByClientId(Guid clientId);

    }
}
