using ChackMate.BLL.Interfaces;
using CheckMate.BLL.Services;
using CheckMate.DAL.Interfaces;
using CheckMate.Domain.Models;

namespace ChackMate.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly AuthService _authService;
        private readonly MailService _mailService;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, AuthService authService, MailService mailService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
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

                // QUESTION : Est-ce une bonne idée d'ajouter le rôle à ce niveau ?
                user.Role = await _roleRepository.GetByName("User")!;

                User userToAdd = await _userRepository.Create(user);

                // Send mail to user with password
                _mailService.SendMail(userToAdd, "Bienvenue sur CheckMate", $"Vous allez pouvoir vous inscrire à tous nos incroyables tournois d'échecs !");

                return userToAdd;
            }
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
    }
}
