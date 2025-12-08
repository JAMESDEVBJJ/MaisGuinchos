using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MaisGuinchos.Models
{
    public class Guincho : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string NomeMotorista { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF tem que ser no formato XXX.XXX.XXX-XX")]
        public string CpfMotorista { get; set; }

        [Required]
        [Phone]
        public string NumeroMotorista { get; set; }

        [MaxLength(100)]
        public string? NomeEmpresa { get; set; }

        [RegularExpression(@"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}", ErrorMessage = "CNPJ tem que ser no formato XX.XXX.XXX/XXXX-XX")]
        public string? CnpjEmpresa { get; set; }

        [Required]
        public string Placa { get; set; }

        [Required]
        public string Modelo { get; set; }

        [Required]
        public string Cor { get; set; }

        [Required]
        [Phone]
        public string TelefoneContato { get; set; }

        [DefaultValue(5)]
        [Range(0, 5)]
        public double Estrelas { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Url]
        public string? Foto { get; set; }
    }
}
