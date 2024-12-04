using CheckMate.Domain.Models;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;

namespace CheckMate.API.DTO
{
    public class UserView
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int Elo { get; set; }
        public Role Role { get; set; }
    }

    public class UserListView
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class UserSelfRegistrationForm
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        /*The regular expression below cheks that a password:
            Has minimum 8 characters in length.Adjust it by modifying {8,}
            At least one uppercase English letter.You can remove this condition by removing (?=.*?[A - Z])
            At least one lowercase English letter.You can remove this condition by removing (?=.*?[a - z])
            At least one digit.You can remove this condition by removing (?=.*?[0 - 9])
            At least one special character,  You can remove this condition by removing (?=.*?[#?!@$%^&*-])*/
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string CheckPassword { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^[MFO]$", ErrorMessage = @"Genre doit être soit 'M', soit 'F' soit 'O'")]
        public string Gender { get; set; }
    }
}
