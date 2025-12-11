using MaisGuinchos.Dtos;
using MaisGuinchos.Services.Interfaces;
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

        public async Task<List<NominatimReturnDTO>?> GetCordsFromAdress(string address)
        {
            string cordsUrl = $"https://nominatim.openstreetmap.org/search?format=json&q={address}";

            var response = await _httpClient.GetAsync(cordsUrl);

            var content = await response.Content.ReadAsStringAsync();

            return System.Text.Json.JsonSerializer.Deserialize<List<NominatimReturnDTO>>(content);
        }

        public async Task<RouteDTO>? GetRouteDistance(string user, string guincho)
        {
            string routeUrl = $"http://router.project-osrm.org/route/v1/driving/{user};{guincho}?overview=false";

            var response = await _httpClient.GetAsync(routeUrl);

            var content = await response.Content.ReadAsStringAsync();

            return System.Text.Json.JsonSerializer.Deserialize<RouteDTO>(content);
        }
    }
}
