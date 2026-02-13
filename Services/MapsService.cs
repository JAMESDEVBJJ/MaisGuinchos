using MaisGuinchos.Dtos;
using MaisGuinchos.Dtos.Route;
using MaisGuinchos.Infrastructure.http.OSRM;
using MaisGuinchos.Services.Interfaces;
using System.Globalization;
using System.Text.Json;

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

            var osrmReturn = JsonSerializer.Deserialize<OsrmResponse>(content);

            var osrmData = new RouteDTO
            {
                durationMin = osrmReturn?.routes?[0].duration / 60,
                distanceKm = osrmReturn?.routes?[0].distance / 1000
            };

            return osrmData;
        }

        public async Task<CalculateRouteReturnDTO?> GetRoute(
                     double originLat,
                     double originLon,
                     double destLat,
                     double destLon)
        {
            string routeUrl =
                $"http://router.project-osrm.org/route/v1/driving/" +
                $"{originLon.ToString(CultureInfo.InvariantCulture)}," +
                $"{originLat.ToString(CultureInfo.InvariantCulture)};" +
                $"{destLon.ToString(CultureInfo.InvariantCulture)}," +
                $"{destLat.ToString(CultureInfo.InvariantCulture)}" +
                $"?overview=full&geometries=geojson";

            var response = await _httpClient.GetAsync(routeUrl);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            var osrmReturn = JsonSerializer.Deserialize<OsrmResponse>(content);

            if (osrmReturn?.routes == null || osrmReturn.routes.Count == 0)
                return null;

            var route = osrmReturn.routes[0];

            var distanceKm = route.distance / 1000;
            var durationMin = route.duration / 60;

            var polyline = route.geometry.coordinates
                .Select(coord => new CoordinateDto
                {
                    Lon = coord[0],
                    Lat = coord[1]
                })
                .ToList();

            var duration = (int)Math.Round(durationMin, 0);

            decimal baseFee = 80;
            decimal pricePerKm = 5;

            if (distanceKm <= 50)
                pricePerKm = 7.0m;
            else if (distanceKm <= 100)         
                pricePerKm = 6.5m;         
            else if (distanceKm <= 400) pricePerKm = 5.5m; 
            else pricePerKm = 4.8m;

            decimal price = baseFee + ((decimal)distanceKm * pricePerKm) + (duration / 4);

            return new CalculateRouteReturnDTO
            {
                DistanceKm = Math.Round(distanceKm, 2),
                DurationMinutes = duration,
                PriceEstimate = Math.Round(price, 2),
                Polyline = polyline
            };
        }
    }
}
