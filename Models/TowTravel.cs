using System.ComponentModel.DataAnnotations;

namespace MaisGuinchos.Models
{
    public class TowTravel : BaseEntity
    {
        public TowRequest TowRequest { get; set; }
        public Guid TowRequestId { get; set; }

        public User Driver { get; set; }
        public Guid DriverId { get; set; }

        [Required]
        public Decimal FinalPrice { get; set; }

        [Required]
        public int EstimatedArrivalTime { get; set; }

        [Required]
        public TowTravelStatus Status { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? EndedAt { get; set; }

        public DateTime? CanceledAt { get; set; }

        [MaxLength(500)]
        public string? CancellationReason { get; set; }
    }

    public enum TowTravelStatus
    {
        GoingToClient = 0,
        Arrived = 1,
        InProgress = 2,
        Finished = 3,
        Cancelled = 4
    }
}
