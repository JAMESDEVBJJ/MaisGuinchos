using System.ComponentModel.DataAnnotations;

namespace MaisGuinchos.Dtos.Guincho
{
    public class TowGuinchoDTO
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        public string? Model { get; set; }

        [StringLength(30)]
        public string? Color { get; set; }

        [RegularExpression(
            @"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$",
            ErrorMessage = "Placa inválida."
        )]
        public string? Plate { get; set; }

        public string? Photo { get; set; }
    }
}
