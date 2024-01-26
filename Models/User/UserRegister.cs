using System.ComponentModel.DataAnnotations;

namespace AgendaLarAPI.Models.User
{
    public class UserRegister
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        [Display(Name = "CPF")]
        public string SocialNumber { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "E-mail Invalido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        [StringLength(100, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password must match.")]
        [Display(Name = "Confirmação de senha")]
        public string ConfirmPassword { get; set; }

        public bool IsValid => !string.IsNullOrEmpty(Name)
                           && !string.IsNullOrEmpty(SocialNumber)
                           && !string.IsNullOrEmpty(Email)
                           && !string.IsNullOrEmpty(Password)
                           && !string.IsNullOrEmpty(ConfirmPassword)
                           && Password == ConfirmPassword;
    }
}
