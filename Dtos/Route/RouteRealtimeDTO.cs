namespace MaisGuinchos.Dtos.Route
{
    public class RouteRealtimeDTO
    {
        public RouteType Type { get; set; }

        public CoordinateDto Origin { get; set; }
        public CoordinateDto Destination { get; set; }

        public double DistanceKm { get; set; }
        public int DurationMinutes { get; set; }
        public decimal PriceEstimate { get; set; }

        public List<CoordinateDto> Polyline { get; set; }
    }
}