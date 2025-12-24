using MaisGuinchos.Models;

namespace MaisGuinchos.Services.Interfaces
{
    public interface IJwtService
    {
        public string GenerateToken(User user);
    }
}
