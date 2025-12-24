using System.ComponentModel.DataAnnotations;

namespace MaisGuinchos.Dtos
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format invalid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characteres.")]
        public string Password { get; set; }
    }
}
