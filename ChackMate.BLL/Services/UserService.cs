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

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, AuthService authService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _authService = authService;
        }

        public User? Create(User user)
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

                if (_userRepository.GetByUsername(user.Username) != null)
                {
                    throw new ArgumentException("Ce nom d'utilisateur est déjà utilisé");
                }

                if (_userRepository.GetByEmail(user.Email) != null)
                {
                    throw new ArgumentException("Cette adresse email existe déjà");
                }

                user.Salt = _authService.GenerateSalt();

                user.Password = _authService.HashPassword(user.Password, user.Salt);

                user.Role = _roleRepository.GetByName("User")!;

                return _userRepository.Create(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la création de l'utilisateur : {ex.Message}");
            }
        }
    }
}
