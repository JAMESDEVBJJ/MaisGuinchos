using MaisGuinchos.Dtos;

namespace MaisGuinchos.Services.Interfaces
{
    public interface IMapsService
    {
        public Task<List<NominatimReturnDTO>?> GetCordsFromAddress(AddressDTO address);
        public Task<RouteDTO>? GetRouteDistance(string user, string guincho);
    }
}
