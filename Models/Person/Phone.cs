using AgendaLarAPI.Models.Base;

namespace AgendaLarAPI.Models.Person
{
    public class Phone : Entity
    {
        public const int NumberMaxLength = 20;

        public Guid PersonId { get; set; }
        public Person Person { get; set; }
        public PhoneType Type { get; set; }
        public string Number { get; set; }
    }
}
