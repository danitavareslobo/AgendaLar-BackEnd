using AgendaLarAPI.Models.People;
using Microsoft.AspNetCore.Identity;

using Model = AgendaLarAPI.Models.People;


namespace AgendaLarAPI.Models.User
{
    public class User : IdentityUser
    {
        public List<Model.Person> People { get; set; } = new();

        public List<Phone> Phones { get; set; } = new();
    }
}
