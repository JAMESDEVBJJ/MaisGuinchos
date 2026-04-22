using MaisGuinchos.Dtos.Route;
using MaisGuinchos.Models;

namespace MaisGuinchos.Services.Interfaces
{
    public interface ITravelService
    {
        public Task<TowTravel?> GetActiveByDriverId(Guid driverId);
        public Task<TowTravel?> GetActiveByClientId(Guid clientId);
        public CoordinateDto ResolveTarget(TowTravel travel);
    }
}
