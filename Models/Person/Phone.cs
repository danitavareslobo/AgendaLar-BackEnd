using AgendaLarAPI.Models.Base;
using AgendaLarAPI.Models.Person.Validators;

namespace AgendaLarAPI.Models.Person
{
    public class Phone : Entity
    {
        public const int NumberMinLength = 20;
        public const int NumberMaxLength = 20;

        public Guid PersonId { get; set; }
        public Person Person { get; set; }
        public PhoneType Type { get; set; }
        public string Number { get; set; }
        public override bool IsValid => Validate();

        private bool Validate()
        {
            Validate(this, new PhoneValidator());
            return ValidationResult.IsValid;
        }
    }
}
