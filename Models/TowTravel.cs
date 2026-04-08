namespace MaisGuinchos.Models
{
    public class TowTravel : BaseEntity
    {
        public Guid TowRequestId { get; set; }

        public Guid DriverId { get; set;  }

        public Decimal Price { get; set; }

        public int EstimatedArrivalTime { get; set; }

        public TowTravelStatus TowTravelStatus { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? EndedAt { get; set; }

        public DateTime? CanceledAt { get; set; }

        public string? CancellationReason { get; set; }
    }

    public enum TowTravelStatus
    {
        Accepted = 0,
        InProgress = 1,
        Completed = 2,
        Canceled = 3
    } // implementar mais status de viagem, para mais controle de fluxo e histórico
}
