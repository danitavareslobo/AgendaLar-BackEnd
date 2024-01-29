using AgendaLarAPI.Models.People;

using Microsoft.AspNetCore.Identity;


namespace AgendaLarAPI.Models.User
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }

        public List<Person> People { get; set; } = new();

        public List<Phone> Phones { get; set; } = new();
    }
}
