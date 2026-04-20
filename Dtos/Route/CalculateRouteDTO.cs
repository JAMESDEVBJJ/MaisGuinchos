using System.ComponentModel.DataAnnotations;

namespace MaisGuinchos.Dtos.Route
{
    public class CalculateRouteDTO
    {
        [Required]
        [Range(-90, 90)]
        public double OriginLat { get; set; }

        [Required]
        [Range(-180, 180)]
        public double OriginLon { get; set; }

        public string? Destination { get; set; }

        [Range(-90, 90)]
        public double? DestinationLat { get; set; }

        [Range(-180, 180)]
        public double? DestinationLon { get; set; }
    }
}
