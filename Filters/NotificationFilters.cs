using AgendaLarAPI.Extensions;
using AgendaLarAPI.Models.Base;
using AgendaLarAPI.Models.Notification;
using AgendaLarAPI.Services;

using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.Json;

namespace AgendaLarAPI.Filters
{
    public class NotificationFilter : IAsyncResultFilter
    {
        private readonly NotificationService _notificationService;

        public NotificationFilter(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                _notificationService.AddNotification(new Notification(NotificationType.Unauthorized, "Não autorizado: Token inválido."));

            if (_notificationService.HasNotifications)
            {
                context.HttpContext.Response.StatusCode = GetStatusCode(_notificationService.Notifications);
                context.HttpContext.Response.ContentType = "application/json";

                var notifications = JsonSerializer.Serialize(new DefaultResponse(_notificationService.Notifications), JsonExtensions.JsonOptions);
                await context.HttpContext.Response.WriteAsync(notifications);

                _notificationService.ClearNotification();

                return;
            }

            await next();
        }

        private static int GetStatusCode(IReadOnlyCollection<Notification> notifications)
        {
            if (notifications.Any(x => x.Type == NotificationType.Timeout))
                return (int)HttpStatusCode.RequestTimeout;

            if (notifications.Any(x => x.Type == NotificationType.Unauthorized))
                return (int)HttpStatusCode.Unauthorized;

            if (notifications.Any(x => x.Type == NotificationType.Warning))
                return (int)HttpStatusCode.BadRequest;

            if (notifications.Any(x => x.Type == NotificationType.Information))
                return (int)HttpStatusCode.Accepted;

            if (notifications.Any(x => x.Type == NotificationType.Success))
                return (int)HttpStatusCode.OK;

            if (notifications.Any(x => x.Type == NotificationType.Error))
                return (int)HttpStatusCode.InternalServerError;

            return (int)HttpStatusCode.BadRequest;
        }
    }
}
