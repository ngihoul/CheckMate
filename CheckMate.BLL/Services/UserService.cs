using CheckMate.BLL.Interfaces;
using CheckMate.BLL.Services;
using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;
using System.Text.RegularExpressions;

namespace CheckMate.BLL.Services
{
    // TODO : try .. catch ... pas utile.
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
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "L'utilisateur ne peut pas être null");
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
            // catch ArgumentException
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la création de l'utilisateur : {ex.Message}");
            }
        }

        public async Task<User?> CreateByAdmin(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "L'utilisateur ne peut pas être null");
                }

                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    throw new ArgumentException("L'email est obligatoire");
                }

                if (await _userRepository.GetByEmail(user.Email) != null)
                {
                    throw new ArgumentException("Cette adresse email existe déjà");
                }

                // Create Password
                string plainPassword = _authService.GeneratePassword();
                // Create Salt
                user.Salt = _authService.GenerateSalt();
                // Create HashPassword
                user.Password = _authService.HashPassword(plainPassword, user.Salt);

                // Add User role
                user.Role = await _roleRepository.GetByName("User")!;

                // Save user
                User? userToAdd = await _userRepository.Create(user);

                // Send mail to user with password
                _mailService.SendMail(userToAdd, "Bienvenue sur CheckMate", $"Connectez-vous avec votre mot de passe : {plainPassword} et choisissez un nom d'utilisateur.");

                return userToAdd;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la création de l'utilisateur : {ex.Message}");
            }
        }

        public async Task<User?> ChooseUsername(int id, User user)
        {
            User? userToPatch = await _userRepository.GetById(id);

            if (userToPatch is null)
            {
                return null;
            }

            userToPatch.Username = user.Username;

            return await _userRepository.Patch(userToPatch);
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

        private bool isEmail(string email)
        {
            Regex emailRegex = new Regex("^\\S+@\\S+\\.\\S+$");

            return emailRegex.IsMatch(email);
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

        public async Task<TournamentCategory>GetUserCategory(User user)
        {
            TournamentCategory userCategory = null;
            List<TournamentCategory> categories = await _categoryRepository.GetAll();
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
    }
}
