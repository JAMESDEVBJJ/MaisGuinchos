namespace MaisGuinchos.Dtos.User
{
    using System.ComponentModel.DataAnnotations;

    public class UpdatePasswordDTO
    {
        [Required(ErrorMessage = "A senha atual é obrigatória.")]
        [MinLength(8, ErrorMessage = "A senha atual deve ter no mínimo 8 caracteres.")]

        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "A nova senha deve ter no mínimo 8 caracteres.")]
        [MaxLength(100)]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&._#\-])[A-Za-z\d@$!%*?&._#\-]{8,}$",
            ErrorMessage = "A nova senha deve conter ao menos uma letra maiúscula, uma minúscula, um número e um caractere especial."
        )]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirme a nova senha.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
