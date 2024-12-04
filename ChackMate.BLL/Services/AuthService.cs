using Azure.Identity;
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

        public string GeneratePassword()
        {
            char[] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789#?!@$%^&*-".ToCharArray();
            string password = "";

            for(int i = 0; i < 18; i++)
            {
                int randomNumber = Random.Shared.Next(0, characters.Length);
                password += characters[randomNumber];
            }

            return password;
        }

        public string HashPassword(string password, string salt)
        {
            return Argon2.Hash(salt + password + _config["Pepper"]);
        }
    }
}
