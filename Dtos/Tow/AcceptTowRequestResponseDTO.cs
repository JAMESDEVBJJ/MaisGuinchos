using MaisGuinchos.Models;
using System.ComponentModel.DataAnnotations;

namespace MaisGuinchos.Dtos.Tow
{
    public class AcceptTowRequestResponseDTO
    {
        [Required]
        public Guid TowRequestId { get; set; }

        [Required]
        public Guid TowTravelId { get; set; }
        
        [Required]
        public Guid TowDriverId { get; set; }

        [Required]
        public decimal FinalPrice { get; set; }

        [Required]
        public int EstimatedArrivalTime { get; set; }

        [Required]
        public double DistanceKm { get; set; }

        [Required]
        public TowRequestStatus TowRequestStatus { get; set; }

        [Required]
        [Range(-90, 90, ErrorMessage = "Latitude do motorista inválida")]
        public double DriverLat { get; set; }

        [Required]
        [Range(-180, 180, ErrorMessage = "Longitude do motorista inválida")]
        public double DriverLon { get; set; }

        [Required]
        [Range(-90, 90, ErrorMessage = "Latitude de origem inválida")]
        public double PickupLat { get; set; }

        [Required]
        [Range(-180, 180, ErrorMessage = "Longitude de origem inválida")]
        public double PickupLon { get; set; }

        [Required]
        [Range(-90, 90, ErrorMessage = "Latitude do destino inválida")]
        public double DestinationLat { get; set; }

        [Required]
        [Range(-180, 180, ErrorMessage = "Longitude do destino inválida")]
        public double DestinationLon { get; set; }
    }
}
