using MaisGuinchos.Dtos;
using MaisGuinchos.Dtos.Route;

namespace MaisGuinchos.Services.Interfaces
{
    public interface IMapsService
    {
        public Task<List<NominatimReturnDTO>?> GetCordsFromAddress(AddressDTO address);
        public Task<RouteDTO>? GetRouteDistance(string user, string guincho);

        public Task<CalculateRouteReturnDTO?> GetRoute(
            double originLat,
            double originLon,
            double? destLat,
            double? destLon);
    }
}
