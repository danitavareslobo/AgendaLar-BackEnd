using System.Text.Json.Serialization;

namespace AgendaLarAPI.Models.People.ViewModels
{
    public class CreatePerson
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string SocialNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public List<CreatePhone>? Phones { get; set; } = new List<CreatePhone>();
    }
}
