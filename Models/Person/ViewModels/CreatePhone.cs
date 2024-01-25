using System.Text.Json.Serialization;

namespace AgendaLarAPI.Models.Person.ViewModels
{
    public class CreatePhone
    {
        [JsonPropertyName("tipo")]
        public PhoneType Type { get; set; }

        [JsonPropertyName("numero")]
        public string Number { get; set; }
    }
}
