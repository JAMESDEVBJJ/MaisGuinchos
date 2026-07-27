using MaisGuinchos.Dtos.Guincho;

namespace MaisGuinchos.Dtos.User
{
    public class UpdateUserProfileDTO
    {
        public string Name { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string NumeroTelefone { get; set; } = string.Empty;

        public IFormFile? Photo { get; set; }

        public TowGuinchoDTO? Guincho { get; set; }
    }
}
