using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Gender { get; set; }
        public int Elo { get; set; } = 1200;
        public Role Role { get; set; }

        public User() {}

        public User(int id, string username, string email, string password, string salt, DateTime dateOfBirth, char gender, int elo, Role role)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
            Salt = salt;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Elo = elo;
            Role = role;
        }
    }

}
