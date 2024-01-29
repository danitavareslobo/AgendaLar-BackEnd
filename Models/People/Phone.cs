using AgendaLarAPI.Models.Base;
using AgendaLarAPI.Models.People.Validators;

namespace AgendaLarAPI.Models.People
{
    public class Phone : Entity
    {
        public const int NumberMinLength = 5;
        public const int NumberMaxLength = 20;

        public Guid PersonId { get; set; }
        public Person Person { get; set; }
        public PhoneType Type { get; set; }
        public string Number { get; set; }
        public User.User User { get; set; }
        public override bool IsValid => Validate();

        private bool Validate()
        {
            Validate(this, new PhoneValidator());
            return ValidationResult.IsValid;
        }
    }
}
