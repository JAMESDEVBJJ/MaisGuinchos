using MaisGuinchos.Models;

namespace MaisGuinchos.Repositorys.Interfaces
{
    public interface ITowRequestRepo
    {
        Task AddAsync(TowRequest request);
        Task<TowRequest?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
    }
}
