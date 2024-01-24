using Microsoft.AspNetCore.Identity;

namespace AgendaLarAPI.Configurations
{
    public class IdentityPortugueseMessages : IdentityErrorDescriber
    {
        public override IdentityError DefaultError() => new()
        {
            Code = nameof(DefaultError),
            Description = "Ocorreu um erro."
        };

        public override IdentityError ConcurrencyFailure() => new()
        {
            Code = nameof(ConcurrencyFailure),
            Description = "Falha de concorrência otimista, o objeto foi modificado."
        };

        public override IdentityError PasswordMismatch() => new()
        {
            Code = nameof(PasswordMismatch),
            Description = "Senha incorreta."
        };

        public override IdentityError InvalidToken() => new()
        {
            Code = nameof(InvalidToken),
            Description = "Token Inválido."
        };

        public override IdentityError LoginAlreadyAssociated() => new()
        {
            Code = nameof(LoginAlreadyAssociated),
            Description = "Já existe um usuário com este login."
        };

        public override IdentityError InvalidUserName(string userName) => new()
        {
            Code = nameof(InvalidUserName),
            Description = $"O nome de usuário '{userName}' é inválido, pode conter apenas letras ou dígitos."
        };

        public override IdentityError InvalidEmail(string email) => new()
        {
            Code = nameof(InvalidEmail),
            Description = $"O email '{email}' é inválido."
        };

        public override IdentityError DuplicateUserName(string userName) => new()
        {
            Code = nameof(DuplicateUserName),
            Description = $"O nome de usuário '{userName}' já está em uso."
        };

        public override IdentityError DuplicateEmail(string email) => new()
        {
            Code = nameof(DuplicateEmail),
            Description = $"O email '{email}' já está em uso."
        };

        public override IdentityError InvalidRoleName(string role) => new()
        {
            Code = nameof(InvalidRoleName),
            Description = $"O nome da role '{role}' é inválido."
        };

        public override IdentityError DuplicateRoleName(string role) => new()
        {
            Code = nameof(DuplicateRoleName),
            Description = $"A role '{role}' já está em uso."
        };

        public override IdentityError UserAlreadyHasPassword() => new()
        {
            Code = nameof(UserAlreadyHasPassword),
            Description = "O usuário já possui uma senha."
        };

        public override IdentityError UserLockoutNotEnabled() => new()
        {
            Code = nameof(UserLockoutNotEnabled),
            Description = "Lockout não está habilitado para este usuário."
        };

        public override IdentityError UserAlreadyInRole(string role) => new()
        {
            Code = nameof(UserAlreadyInRole),
            Description = $"O usuário já possui a role '{role}'."
        };

        public override IdentityError UserNotInRole(string role) => new()
        {
            Code = nameof(UserNotInRole),
            Description = $"O usuário não possui a role '{role}'."
        };

        public override IdentityError PasswordTooShort(int length) => new()
        {
            Code = nameof(PasswordTooShort),
            Description = $"A senha deve conter pelo menos {length} caracteres."
        };

        public override IdentityError PasswordRequiresNonAlphanumeric() => new()
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = "A senha deve conter pelo menos um caractere não alfanumérico."
        };

        public override IdentityError PasswordRequiresDigit() => new()
        {
            Code = nameof(PasswordRequiresDigit),
            Description = "A senha deve conter pelo menos um dígito ('0'-'9')."
        };

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => new()
        {
            Code = nameof(PasswordRequiresUniqueChars),
            Description = $"A senha deve conter pelo menos {uniqueChars} caracteres únicos."
        };

        public override IdentityError PasswordRequiresLower() => new()
        {
            Code = nameof(PasswordRequiresLower),
            Description = "A senha deve conter pelo menos um caractere em caixa baixa ('a'-'z')."
        };

        public override IdentityError PasswordRequiresUpper() => new()
        {
            Code = nameof(PasswordRequiresUpper),
            Description = "A senha deve conter pelo menos um caractere em caixa alta ('A'-'Z')."
        };

        public override IdentityError RecoveryCodeRedemptionFailed() => new()
        {
            Code = nameof(RecoveryCodeRedemptionFailed),
            Description = "Falha ao resgatar o código de recuperação."
        };
    }
}
