using System.ComponentModel.DataAnnotations;

namespace MaisGuinchos.Dtos.Tow
{
    public class CreateTowRequestDto
    {
        [Required]
        public Guid DriverId { get; set; }

        [Required]
        public double PickupLat { get; set; }

        [Required]
        public double PickupLon { get; set; }

        [Required]
        public double DropoffLat { get; set; }

        [Required]
        public double DropoffLon { get; set; }

        [Required]
        public double TotalDistanceKm { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        [Required]
        public decimal SuggestedPrice { get; set; }

        public string? VehicleType { get; set; }
        public string? VehicleIssue { get; set; }
        public string? Notes { get; set; }
    }
}
