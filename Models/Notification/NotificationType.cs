using System.ComponentModel;

namespace AgendaLarAPI.Models.Notification
{
    public enum NotificationType
    {
        [Description("Sucesso")] Success = 1,

        [Description("Informação")] Information = 2,

        [Description("Aviso")] Warning = 3,

        [Description("Erro")] Error = 4,

        [Description("Timeout")] Timeout = 5,

        [Description("Não autorizado")] Unauthorized = 6
    }
}
