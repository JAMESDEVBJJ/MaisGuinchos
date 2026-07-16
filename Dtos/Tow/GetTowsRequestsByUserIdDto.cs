using MaisGuinchos.Models;

namespace MaisGuinchos.Dtos.Tow
{
    public class GetTowsRequestsByUserIdDTO
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;

        public Guid DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;

        public string PickupAddress { get; set; } = string.Empty;
        public double PickupLat { get; set; }
        public double PickupLon { get; set; }
        
        public string DropoffAddress { get; set; } = string.Empty;
        public double DropoffLat { get; set; }
        public double DropoffLon { get; set; }

        public double? DistanceToPickupKm { get; set; }
        public double? DistanceToDestinationKm { get; set; }
        public double TotalDistanceKm { get; set; }

        public int? DurationMinToPickup { get; set; }
        public int? DurationMinToDestination { get; set; }
        public int DurationMinutes { get; set; }

        public decimal SuggestedPrice { get; set; }
        public decimal? FinalPrice { get; set; }

        public string? VehicleType { get; set; }
        public string? VehicleIssue { get; set; }
        public string? Notes { get; set; }

        public decimal? CounterOfferPrice { get; set; }
        public decimal? CounterOfferPercent { get; set; }
        public string? CounterOfferReason { get; set; }
        public DateTime? CounterOfferAt { get; set; }

        public TowRequestStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool HasTravel => Status == TowRequestStatus.Accepted;
    }
}