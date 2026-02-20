using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaisGuinchos.Models
{

    public class TowRequest : BaseEntity
    {
        [Required]
        public Guid ClientId { get; set; }
        public User Client { get; set; }


        [Required]
        public Guid DriverId { get; set; }
        public User Driver { get; set; }

        [Required]
        [Range(-90, 90)]
        public double PickupLat { get; set; }

        [Required]
        [Range(-180, 180)]
        public double PickupLon { get; set; }

        [Required]
        [Range(-90, 90)]
        public double DropoffLat { get; set; }

        [Required]
        [Range(-180, 180)]
        public double DropoffLon { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double TotalDistanceKm { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int DurationMinutes { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal SuggestedPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? FinalPrice { get; set; }

        [MaxLength(100)]
        public string? VehicleType { get; set; }

        [MaxLength(200)]
        public string? VehicleIssue { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        [Required]
        public TowRequestStatus Status { get; set; }
    }
    public enum TowRequestStatus
    {
        WaitingDriverResponse = 1,
        Negotiating = 2,
        Accepted = 3,
        Rejected = 4,
        Cancelled = 5
    }
}
