namespace MaisGuinchos.Dtos.User
{
    public class UpdateUserProfileResponseDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string NumeroTelefone { get; set; } = string.Empty;

        public string Cpf { get; set; } = string.Empty;

        public string Tipo { get; set; } = string.Empty;
    }
}
