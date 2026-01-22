using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MaisGuinchos.Models
{
    public class User : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [RegularExpression("^\\d{3}\\.\\d{3}\\.\\d{3}-\\d{2}$", ErrorMessage = "CPF tem que ser no formato XXX.XXX.XXX-XX")]
        public string Cpf { get; set; }

        [Required]
        [Phone]
        public string NumeroTelefone { get; set; }

        [DefaultValue(5)]
        [Range(0, 5)]
        public double Estrelas { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public ICollection<Location>? Locations { get; set; }

        [Required]
        public UserType Tipo { get; set; }

        public enum UserType
        {
            Cliente = 0,
            Motorista = 1,
            Empresa = 2
        }

        public Guincho? Guincho { get; set; }
    }
}
