using System.Text.Json.Serialization;

namespace AgendaLarAPI.Models.People.ViewModels
{
    public class UpdatePhone
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("personId")]
        public Guid PersonId { get; set; }

        [JsonPropertyName("tipo")]
        public PhoneType Type { get; set; }

        [JsonPropertyName("numero")]
        public string Number { get; set; }
    }
}
