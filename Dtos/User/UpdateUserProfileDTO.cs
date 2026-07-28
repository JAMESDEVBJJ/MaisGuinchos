using System.ComponentModel.DataAnnotations;
using MaisGuinchos.Dtos.Guincho;

namespace MaisGuinchos.Dtos.User
{
    public class UpdateUserProfileDTO
    {
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        public string? Name { get; set; }

        [StringLength(30, MinimumLength = 3,
            ErrorMessage = "O nome de usuário deve ter entre 3 e 30 caracteres.")]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Telefone inválido.")]
        public string? NumeroTelefone { get; set; }

        public IFormFile? Photo { get; set; }

        public TowGuinchoDTO? Guincho { get; set; }
    }
}