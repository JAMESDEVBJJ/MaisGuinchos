namespace MaisGuinchos.Dtos.Tow
{
    public class PutTowCancelCounterOfferDTO
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }
        public string ClientName { get; set; }

        public Guid? DriverId { get; set; }
        public string? DriverName { get; set; }

        public double PickupLat { get; set; }
        public double PickupLon { get; set; }

        public double DropoffLat { get; set; }
        public double DropoffLon { get; set; }

        public double TotalDistanceKm { get; set; }
        public int DurationMinutes { get; set; }

        public decimal SuggestedPrice { get; set; }
        public decimal? FinalPrice { get; set; }

        public decimal? CounterOfferPrice { get; set; }
        public decimal? CounterOfferPercent { get; set; }
        public string? CounterOfferReason { get; set; }
        public DateTime? CounterOfferAt { get; set; }

        public int Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
