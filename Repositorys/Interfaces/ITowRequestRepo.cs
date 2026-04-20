using MaisGuinchos.Models;

namespace MaisGuinchos.Repositorys.Interfaces
{
    public interface ITowRequestRepo
    {
        Task AddAsync(TowRequest request);

        Task<TowRequest?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();

        Task<List<TowRequest>> GetPendingsAsync(Guid driverId);

        Task<TowRequest> UpdateAsync(TowRequest towRequest);

        Task<bool> HasActiveRequestAsync(Guid clientId, Guid driverId);
        Task<TowRequest?> GetByIdIncludeLocations(Guid idTowRequest);
    }
}
