namespace MaisGuinchos.Dtos.Route
{
    public class CalculateRouteReturnDTO
    {
        public double DistanceKm { get; set; }
        public int DurationMinutes { get; set; }
        public decimal PriceEstimate { get; set; }
        public List<CoordinateDto> Polyline { get; set; }
    }

    public class CoordinateDto
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
