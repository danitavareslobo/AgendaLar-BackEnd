using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using AgendaLarAPI.Models.Notification;
using AgendaLarAPI.Services;
using AgendaLarAPI.Extensions;
using AgendaLarAPI.Models;

namespace AgendaLarAPI.Controllers.Base
{
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public DefaultController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        protected ActionResult CustomResponse(object? result = null)
        {
            if (ValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    result
                });
            }

            return BadRequest(new ValidationProblemDetails(
                new Dictionary<string, string[]>
            {
            { "Mensagens", GetErros() }
            })
            {
                Title = "Ocorreu um ou mais erros de validação."
            });
        }

        private string[] GetErros()
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMsg);
            }

            return _notificationService.Notifications.Select(n => n.Message).ToArray();
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotifyErrorModelInvalid(modelState);
            return CustomResponse();
        }

        protected bool ValidOperation()
        {
            return !ModelState.Values.Any(v => v.Errors.Count > 0) &&
                   !_notificationService.HasNotifications;
        }

        protected void NotifyErrorModelInvalid(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(errorMsg);
            }
        }

        protected void NotifyError(string mensagem, string title = "Ocorreu um erro.", NotificationType type = NotificationType.Error)
        {
            _notificationService.AddNotification(title, mensagem, NotificationType.Error);
            _notificationService.AddNotification(title, mensagem, type);
        }

        protected string LoggedUserId => GetLoggedUserId();

        private string GetLoggedUserId()
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == AgendaConstants.NameIdentifierClaimType);
            var userId = claim?.Value;

            if (!string.IsNullOrWhiteSpace(userId)) return userId;

            NotifyError(
                NotificationType.Unauthorized.GetEnumDescription(),
                "Usuário não autenticado.",
                NotificationType.Unauthorized);
            return string.Empty;
        }
    }
}
