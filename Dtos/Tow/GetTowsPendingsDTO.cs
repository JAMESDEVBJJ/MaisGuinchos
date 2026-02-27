using MaisGuinchos.Models;

namespace MaisGuinchos.Dtos.Tow
{
    public class GetTowsPendingsDTO
    {
        public Guid Id { get; set; }

        public string ClientName { get; set; }

        public Guid ClientId { get; set; }

        public double PickupLat { get; set; }
        public double PickupLon { get; set; }

        public double DropoffLat { get; set; }
        public double DropoffLon { get; set; }

        public double TotalDistanceKm { get; set; }

        public int DurationMinutes { get; set; }

        public decimal SuggestedPrice { get; set; }

        public string? VehicleType { get; set; }

        public string? VehicleIssue { get; set; }

        public string? Notes { get; set; }

        public TowRequestStatus Status { get; set; }
    }
}
