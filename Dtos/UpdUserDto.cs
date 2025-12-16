using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MaisGuinchos.Dtos
{
    public class UpdUserDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? UserName { get; set; }

        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$",
            ErrorMessage = "CPF deve estar no formato XXX.XXX.XXX-XX")]
        public string? Cpf { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? NumeroTelefone { get; set; }

        [MinLength(8)]
        public string? Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
