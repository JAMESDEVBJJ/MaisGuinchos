using MaisGuinchos.Dtos;
using MaisGuinchos.Models;

namespace MaisGuinchos.Repositorys.Interfaces
{
    public interface ILocationRepo
    {
        public Task<Location> UpdateLocation(int id, CreateLocationDTO location);
    }
}
