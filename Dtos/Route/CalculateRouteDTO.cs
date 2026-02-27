using System.ComponentModel.DataAnnotations;

namespace MaisGuinchos.Dtos.Route
{
    public class CalculateRouteDTO
    {
        [Required]
        public double OriginLat { get; set; }

        [Required]
        public double OriginLon { get; set; }

        public string? Destination { get; set; }

        public double? DriverLat { get; set; }
        public double? DriverLon { get; set; }
    }
}
