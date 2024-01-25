using FluentValidation;

namespace AgendaLarAPI.Models.Person.Validators
{
    public class PhoneValidator : AbstractValidator<Phone>
    {
        public PhoneValidator()
        {
            RuleFor(x => x.Number)
                .NotEmpty()
                .WithMessage("O número do telefone é obrigatório.")
                .MaximumLength(Phone.NumberMaxLength)
                .WithMessage("O número do telefone não pode ter mais que {MaxLength} caracteres.")
                .MinimumLength(Phone.NumberMinLength)
                .WithMessage("O número do telefone deve ter pelo menos {MinLength} caracteres.");

            RuleFor(x => x.PersonId)
                .NotEmpty()
                .WithMessage("O Id da pessoa é obrigatório.");

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("O tipo do telefone é obrigatório.")
                .IsInEnum()
                .WithMessage("O tipo do telefone é inválido.");
        }
    }
}
