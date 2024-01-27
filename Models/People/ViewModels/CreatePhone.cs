using System.Text.Json.Serialization;

namespace AgendaLarAPI.Models.People.ViewModels
{
    public class CreatePhone
    {
        [JsonPropertyName("tipo")]
        public PhoneType Type { get; set; }

        [JsonPropertyName("numero")]
        public string Number { get; set; }
    }
}
