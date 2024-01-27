using System.Text.Json.Serialization;

namespace AgendaLarAPI.Models.People.ViewModels
{
    public class CreatePerson
    {
        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("cpf")]
        public string SocialNumber { get; set; }

        [JsonPropertyName("dataNascimento")]
        public DateTime BirthDate { get; set; }

        [JsonPropertyName("telefones")]
        public List<CreatePhone>? Phones { get; set; } = new List<CreatePhone>();
    }
}
