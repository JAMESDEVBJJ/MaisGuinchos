using MaisGuinchos.Dtos;
using MaisGuinchos.Infrastructure.http.OSRM;
using MaisGuinchos.Services.Interfaces;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace MaisGuinchos.Services
{
    public class MapsService : IMapsService
    {
        private readonly HttpClient _httpClient;

        public MapsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MaisGuinchosApp/1.0 (contato: jamescblbjj@gmail.com)");
        }

        public async Task<List<NominatimReturnDTO>?> GetCordsFromAddress(AddressDTO address)
        {
            string cordsUrl = $"https://nominatim.openstreetmap.org/search?format=json&q={address.address}";

            var response = await _httpClient.GetAsync(cordsUrl);

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<NominatimReturnDTO>>(content);
        }

        public async Task<RouteDTO>? GetRouteDistance(string user, string guincho)
        {
            string routeUrl = $"http://router.project-osrm.org/route/v1/driving/{user};{guincho}?overview=false";

            var response = await _httpClient.GetAsync(routeUrl);

            var content = await response.Content.ReadAsStringAsync();

            var osrmReturn = JsonSerializer.Deserialize<OsrmReponse>(content);

            var osrmData = new RouteDTO
            {
                durationMin = osrmReturn?.routes?[0].duration / 60,
                distanceKm = osrmReturn?.routes?[0].distance / 1000
            };

            return osrmData;
        }
    }
}
