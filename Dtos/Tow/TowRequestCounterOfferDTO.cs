namespace MaisGuinchos.Dtos.Tow
{
    public class TowRequestCounterOfferDto
    {
        public decimal NewPrice { get; set; }

        public decimal InitialPrice { get; set; }

        public decimal Percent { get; set; }
        public string? Reason { get; set; }
    }
}
