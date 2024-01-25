using AgendaLarAPI.Models.Base;

namespace AgendaLarAPI.Models.Person
{
    public class Person : Entity
    {
        public string Name { get; set; }
        public string SocialNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public List<Phone> Phones { get; set; }
    }
}
