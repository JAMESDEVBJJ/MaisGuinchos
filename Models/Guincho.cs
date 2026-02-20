using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MaisGuinchos.Models
{
    public class Guincho : BaseEntity
    {

        public Guid UserId { get; set; }
        public User User { get; set; }

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

        [Url]
        public string? Foto { get; set; }

        public bool Disponivel { get; set; }
    }
}
