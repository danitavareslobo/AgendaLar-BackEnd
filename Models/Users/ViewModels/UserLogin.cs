using AgendaLarAPI.Extensions;

using System.ComponentModel.DataAnnotations;

namespace AgendaLarAPI.Models.User.ViewModels
{
    public class UserLogin
    {
        [Required(ErrorMessage = "E-mail é obrigatório")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "E-mail Invalido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        [StringLength(100, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        public bool IsEmpty => string.IsNullOrEmpty(Email)
                           || string.IsNullOrEmpty(Password);

        public bool IsValidEmail => Email.IsValidEmail();

        public bool IsValidPassword => Password.IsValidPassword();
    }

}
