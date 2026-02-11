using System.ComponentModel.DataAnnotations;

namespace MaisGuinchos.Dtos.Route
{
    public class CalculateRouteDTO
    {
        [Required]
        public double OriginLat { get; set; }

        [Required]
        public double OriginLon { get; set; }

        [Required]
        public string Destination { get; set; }

        //no futuro
        public double? DriverLat { get; set; }
        public double? DriverLon { get; set; }
    }
}
