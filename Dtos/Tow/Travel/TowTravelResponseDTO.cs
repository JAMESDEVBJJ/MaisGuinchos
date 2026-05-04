using MaisGuinchos.Models;

namespace MaisGuinchos.Dtos.Tow.Travel
{
    public class TowTravelResponseDTO
    {
        public Guid Id { get; set; }

        public Guid DriverId { get; set; }
        public string DriverName { get; set; }

        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string VehicleModel { get; set; }
        public string Notes { get; set; }

        public string Questions { get; set; }

        public Guid TowRequestId { get; set; }

        public decimal FinalPrice { get; set; }

        public double? DistanceToPickupKm { get; set; }
        public int? TimeToPickupMin { get; set; }

        public double? DistanceToDestinationKm { get; set; }
        public int? TimeToDestinationMin { get; set; }

        public TowTravelStatus Status { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public DateTime? CanceledAt { get; set; }
        public string? CancellationReason { get; set; }

        public LocationDTO Origin { get; set; }
        public LocationDTO Destination { get; set; }
    }
}
