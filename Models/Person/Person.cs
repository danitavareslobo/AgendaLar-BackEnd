using AgendaLarAPI.Models.Base;

namespace AgendaLarAPI.Models.Person
{
    public class Person : Entity
    {
        public const int NameMaxLength = 100;
        public const int EmailMaxLength = 100;
        public const int SocialNumberMaxLength = 18;

        public string Name { get; set; }
        public string Email { get; set; }
        public string SocialNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Phone> Phones { get; set; }
    }
}
