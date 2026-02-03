namespace MaisGuinchos.Dtos.User
{
    using System.ComponentModel.DataAnnotations;

    public class CreateUserDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 150 caracteres.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome de usuário deve ter entre 3 e 50 caracteres.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        //[RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter 11 números.")] nn conta . e -
        public string Cpf { get; set; } = null!;

        [Required(ErrorMessage = "O número de telefone é obrigatório.")]
        [RegularExpression(@"^\d{10,13}$", ErrorMessage = "O telefone deve conter DDD e número.")]
        public string NumeroTelefone { get; set; } = null!;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        [Range(0, 2, ErrorMessage = "Tipo de usuário inválido.")]
        public int Tipo { get; set; }

        public CreateGuinchoRequest? Guincho { get; set; }
    }

}
