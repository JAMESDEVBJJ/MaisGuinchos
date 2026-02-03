using System.ComponentModel.DataAnnotations;

namespace MaisGuinchos.Dtos.User
{
    public class UserAddedDTO
    {
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string NumeroTelefone { get; set; }

        public UserType Tipo { get; set; }

        public enum UserType
        {
            Cliente = 0,
            Motorista = 1,
            Empresa = 2
        }

        public CreateGuinchoRequest? Guincho {get; set;}
    }
}
