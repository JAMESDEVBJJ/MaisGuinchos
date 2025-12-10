using BCrypt.Net;

namespace MaisGuinchos.utils
{
    public class PasswordHasher
    {
        public string Hasher(string password) 
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hashPassword) 
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
    }
}
