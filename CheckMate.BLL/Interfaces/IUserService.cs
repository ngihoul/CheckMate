﻿using CheckMate.Domain.Models;

namespace CheckMate.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<User?> Create(User user);
        public Task<User?> CreateByAdmin(User user);
        public Task<User?> ChooseUsername(int id, User user);
        public Task<User?> Login(string usernameOrEmail, string password);
        public int GetAge(User user);
        public Task<TournamentCategory> GetUserCategory(User user);
    }
}
