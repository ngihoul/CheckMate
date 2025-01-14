﻿using CheckMate.BLL.Interfaces;
using CheckMate.BLL.Services;
using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;
using System.Text.RegularExpressions;

namespace CheckMate.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITournamentCategoryRepository _categoryRepository;
        private readonly AuthService _authService;
        private readonly MailService _mailService;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, ITournamentCategoryRepository categoryRepository, AuthService authService, MailService mailService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _categoryRepository = categoryRepository;
            _authService = authService;
            _mailService = mailService;
        }

        public async Task<User?> Create(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException("L'utilisateur ne peut pas être null");
            }

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new ArgumentException("Le nom d'utilisateur est obligatoire");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("L'email est obligatoire");
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("Le mot de passe est obligatoire");
            }

            if (await _userRepository.GetByUsername(user.Username) != null)
            {
                throw new ArgumentException("Ce nom d'utilisateur est déjà utilisé");
            }

            if (await _userRepository.GetByEmail(user.Email) != null)
            {
                throw new ArgumentException("Cette adresse email est déjà utilisée");
            }

            user.Salt = _authService.GenerateSalt();

            user.Password = _authService.HashPassword(user.Password, user.Salt);

            User userToAdd = await _userRepository.Create(user);

            // Send mail to user with password
            _mailService.SendMail(userToAdd, "Bienvenue sur CheckMate", $"Vous allez pouvoir vous inscrire à tous nos incroyables tournois d'échecs !");

            return userToAdd;

        }

        public async Task<User?> CreateByAdmin(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException("L'utilisateur ne peut pas être null");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("L'email est obligatoire");
            }

            if (await _userRepository.GetByEmail(user.Email) != null)
            {
                throw new ArgumentException("Cette adresse email existe déjà");
            }

            string plainPassword = _authService.GeneratePassword();
            user.Salt = _authService.GenerateSalt();
            user.Password = _authService.HashPassword(plainPassword, user.Salt);

            User? userToAdd = await _userRepository.Create(user);

            _mailService.SendMail(userToAdd, "Bienvenue sur CheckMate", $"Connectez-vous avec votre mot de passe : {plainPassword} et choisissez un nom d'utilisateur.");

            return userToAdd;
        }

        public async Task<User?> InitAccount(int id, User user)
        {
            User? userToPatch = await _userRepository.GetById(id);

            if (userToPatch is null)
            {
                throw new ArgumentNullException("L'utilisateur ne peut pas être null");
            }

            userToPatch.Username = user.Username;
            userToPatch.Salt = _authService.GenerateSalt();
            userToPatch.Password = _authService.HashPassword(user.Password, userToPatch.Salt);

            return await _userRepository.Patch(userToPatch);
        }
        public async Task<List<User>> GetByCategories(IEnumerable<TournamentCategory> categories)
        {
            if(categories.Count() <= 0)
            {
                throw new ArgumentNullException("Les categories ne peuvent pas etre null");
            }

            return await _userRepository.GetByCategories(categories);
        }

        public async Task<User?> Login(string usernameOrEmail, string password)
        {
            User? user = null;

            if (isEmail(usernameOrEmail))
            {
                user = await _userRepository.GetByEmailForLogin(usernameOrEmail);
            }
            else
            {
                user = await _userRepository.GetByUsernameForLogin(usernameOrEmail);
            }

            if (user is null || !_authService.Verify(user, password))
            {
                throw new Exception("Données invalides");
            }

            return user;
        }

        public int GetAge(User user)
        {
            int age = 0;

            age = DateTime.Now.Year - user.DateOfBirth.Year;

            if (DateTime.Now.Month < user.DateOfBirth.Month || (DateTime.Now.Month == user.DateOfBirth.Month && DateTime.Now.Day < user.DateOfBirth.Day))
            {
                age--;
            }

            return age;
        }

        public async Task<TournamentCategory> GetUserCategory(User user)
        {
            TournamentCategory? userCategory = null;
            IEnumerable<TournamentCategory> categories = await _categoryRepository.GetAll();
            int userAge = GetAge(user);

            if (userAge < 18)
            {
                userCategory = categories.Where(c => c.Name == "Junior").FirstOrDefault();
            }
            else if (userAge >= 18 && userAge < 60)
            {
                userCategory = categories.Where(c => c.Name == "Senior").FirstOrDefault();
            }
            else
            {
                userCategory = categories.Where(c => c.Name == "Veteran").FirstOrDefault();
            }

            return userCategory;
        }

        private bool isEmail(string email)
        {
            Regex emailRegex = new Regex("^\\S+@\\S+\\.\\S+$");

            return emailRegex.IsMatch(email);
        }
    }
}
