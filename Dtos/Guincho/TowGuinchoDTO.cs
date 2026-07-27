namespace MaisGuinchos.Dtos.Guincho
{
    public class TowGuinchoDTO
    {
        public Guid Id { get; set; }

        public string Model { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string Plate { get; set; } = string.Empty; 

        public string? Photo { get; set; }
    }
}
