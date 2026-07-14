using MaisGuinchos.Models;

namespace MaisGuinchos.Dtos.Tow.Travel
{
    public class TowTravelHistoryResponseDTO
    {
        public Guid Id { get; set; }

        public Guid TowRequestId { get; set; }

        public Guid DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;

        public string driverPhone { get; set; } = string.Empty;

        public string driverTow { get; set; }

        public Guid ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;

        public decimal FinalPrice { get; set; }

        public double? DistanceToPickupKm { get; set; }
        public int? TimeToPickupMin { get; set; }

        public double? DistanceToDestinationKm { get; set; }
        public int? TimeToDestinationMin { get; set; }

        public TowTravelStatus Status { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public DateTime? CanceledAt { get; set; }

        public string CancellationReason { get; set; } = string.Empty;
    }
}
