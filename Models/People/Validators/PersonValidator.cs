using FluentValidation;

namespace AgendaLarAPI.Models.People.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O Nome é obrigatório.")
                .MaximumLength(Person.NameMaxLength)
                .WithMessage("O Nome não pode ter mais que {MaxLength} caracteres.")
                .MinimumLength(Person.NameMinLength)
                .WithMessage("O Nome deve ter pelo menos {MinLength} caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(Person.EmailMaxLength)
                .WithMessage("O {PropertyName} não pode ter mais que {MaxLength} caracteres.")
                .MinimumLength(Person.EmailMinLength)
                .WithMessage("O {PropertyName} deve ter pelo menos {MinLength} caracteres.")
                .EmailAddress()
                .WithMessage("O {PropertyName} não é válido.");

            RuleFor(x => x.SocialNumber)
                .NotEmpty()
                .WithMessage("O CPF é obrigatório.")
                .MaximumLength(Person.SocialNumberMaxLength)
                .WithMessage("O CPF não pode ter mais que {MaxLength} caracteres.")
                .MinimumLength(Person.SocialNumberMinLength)
                .WithMessage("O CPF deve ter pelo menos {MinLength} caracteres.");

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .WithMessage("A data de nascimento é obrigatória.")
                .LessThan(DateTime.UtcNow)
                .WithMessage("A data de nascimento não pode ser maior que a data atual.");
        }
    }
}
