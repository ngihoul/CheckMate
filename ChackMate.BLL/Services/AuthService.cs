using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Configuration;

namespace CheckMate.BLL.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateSalt()
        {
            return Guid.NewGuid().ToString();
        }

        public string HashPassword(string password, string salt)
        {
            return Argon2.Hash(salt + password + _config["Pepper"]);
        }
    }
}
