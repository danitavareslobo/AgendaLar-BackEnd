using System.Text.Json.Serialization;

namespace AgendaLarAPI.Models.Notification
{
    public class Notification
    {
        public Notification(string key, string message, NotificationType type = NotificationType.Information)
        {
            Key = key;
            Type = type;
            Message = message;
        }

        public Notification(NotificationType type, string message)
        {
            Key = type.GetEnumDescription();
            Message = message;
        }

        [JsonPropertyName("key")]
        public string Key { get; }

        [JsonIgnore]
        public NotificationType Type { get; }

        [JsonPropertyName("message")]
        public string Message { get; }
    }
}
