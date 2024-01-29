using System.Text.Json.Serialization;

namespace AgendaLarAPI.Models.People.ViewModels
{
    public class PersonResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("socialNumber")]
        public string SocialNumber { get; set; }

        [JsonPropertyName("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonPropertyName("ativo")]
        public bool Ativo { get; set; }

        [JsonPropertyName("phones")]
        public List<UpdatePhone> Phones { get; set; } = new();
    }
}
