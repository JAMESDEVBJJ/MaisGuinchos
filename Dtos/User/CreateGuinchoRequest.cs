namespace MaisGuinchos.Dtos.User
{
    using System.ComponentModel.DataAnnotations;

    public class CreateGuinchoRequest
    {
        [Required(ErrorMessage = "O modelo do guincho é obrigatório.")]
        [StringLength(100, ErrorMessage = "O modelo pode ter no máximo 100 caracteres.")]
        public string Modelo { get; set; } = null!;

        [Required(ErrorMessage = "A placa do guincho é obrigatória.")]
        [RegularExpression(
            @"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$",
            ErrorMessage = "Placa inválida. Use o padrão Mercosul (ex: BRA1A23)."
        )]
        public string Placa { get; set; } = null!;

        [Required(ErrorMessage = "A cor do guincho é obrigatória.")]
        [StringLength(30, ErrorMessage = "A cor pode ter no máximo 30 caracteres.")]
        public string Cor { get; set; } = null!;

        //[Required(ErrorMessage = "A CNH é obrigatória.")]  bota no modelo
        [RegularExpression(
            @"^\d{11}$",
            ErrorMessage = "A CNH deve conter exatamente 11 números."
        )]
        public string? Cnh { get; set; } = null!;

        public IFormFile? Foto { get; set; }
    }
}
