using AgendaLarAPI.Models.Base;
using AgendaLarAPI.Models.People.Validators;
using Model = AgendaLarAPI.Models.User;
using FluentValidation.Results;

namespace AgendaLarAPI.Models.People
{
    public class Person : Entity
    {
        public const int NameMinLength = 5;
        public const int NameMaxLength = 100;
        public const int EmailMinLength = 5;
        public const int EmailMaxLength = 100;
        public const int SocialNumberMinLength = 11;
        public const int SocialNumberMaxLength = 18;

        public string Name { get; set; }
        public string Email { get; set; }
        public string SocialNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Phone> Phones { get; set; }
        public Model.User User { get; set; }

        public override bool IsValid => Validate();

        private bool Validate()
        {
            Validate(this, new PersonValidator());
            return ValidationResult.IsValid;
        }
    }
}
