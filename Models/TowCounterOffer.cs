using System.ComponentModel.DataAnnotations;


namespace MaisGuinchos.Models
{
    public class TowCounterOffer
    {
        public Guid Id { get; set; }

        [Required]
        public Guid TowRequestId { get; set; }

        [Required]
        public Guid DriverId { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal OriginalPrice { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal NewPrice { get; set; }

        [Required]
        [Range(0, 15)]
        public int PercentageIncrease { get; set; }

        [Required]
        [MaxLength(200)]
        public string Reason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool? Accepted { get; set; }
    }
}
