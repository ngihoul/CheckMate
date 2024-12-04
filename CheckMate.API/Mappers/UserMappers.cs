using CheckMate.API.DTO;
using CheckMate.Domain.Models;

namespace CheckMate.API.Mappers
{
    public static class UserMappers
    {
        public static User ToUser(this UserSelfRegistrationForm userForm)
        {
            return new User
            {
                Username = userForm.Username,
                Email = userForm.Email,
                Password = userForm.Password,
                DateOfBirth = userForm.DateOfBirth,
                Gender = Convert.ToChar(userForm.Gender)
            };
        }

        public static UserView ToView(this User user)
        {
            return new UserView
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender.ToString(),
                Elo = user.Elo,
                Role = user.Role
            };
        }
    }
}
