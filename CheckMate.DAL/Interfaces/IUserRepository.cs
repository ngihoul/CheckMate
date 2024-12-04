using CheckMate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.DAL.Interfaces
{
    public interface IUserRepository
    {
        public User? Create(User user);
        public User? GetByEmail(string email);
        public User? GetByUsername(string username);
    }
}
