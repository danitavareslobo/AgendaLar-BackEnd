using AgendaLarAPI.Models.Notification;
using System.Text.Json.Serialization;
using Model = AgendaLarAPI.Models.Notification;

namespace AgendaLarAPI.Models.Base
{
    public class DefaultResponse
    {
        public DefaultResponse()
        {

        }

        public DefaultResponse(ICollection<Model.Notification> notifications)
        {
            var isValid = notifications.Count(a => a.Type == NotificationType.Information) == notifications.Count;
            Success = notifications.Count == 0 || isValid;
            Message = Success ? "Operação realizada com sucesso." : "Ocorreu um erro ao processar a requisição.";
            Data = notifications;
        }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }
    }
}
