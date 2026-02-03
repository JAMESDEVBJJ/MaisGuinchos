using MaisGuinchos.Dtos;
using MaisGuinchos.Models;

namespace MaisGuinchos.Repositorys.Interfaces
{
    public interface ILocationRepo
    {
        public Task<Location> UpdateLocation(Guid id, CreateLocationDTO location);

        public Task<Location?> GetLastFromUser(Guid userId);
    }
}
