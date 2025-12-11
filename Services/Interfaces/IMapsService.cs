using MaisGuinchos.Dtos;

namespace MaisGuinchos.Services.Interfaces
{
    public interface IMapsService
    {
        public Task<List<NominatimReturnDTO>?> GetCordsFromAdress(string address);
        public Task<RouteDTO>? GetRouteDistance(string user, string guincho);
    }
}
