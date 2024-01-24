using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace AgendaLarAPI.Controllers.Base
{
    [ApiController]
    public class DefaultController : ControllerBase
    {
        protected ICollection<string> Errors = new List<string>();

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
                { "Mensagens", GetErrors() }
                })
            {
                Title = "Ocorreu um ou mais erros de validação."
            });
        }

        private string[] GetErrors()
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMsg);
            }

            return Errors.ToArray();
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotifyErrorModelInvalid(modelState);
            return CustomResponse();
        }

        protected bool ValidOperation()
        {
            return !ModelState.Values.Any(v => v.Errors.Count > 0) &&
                   !Errors.Any();
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

        protected void NotifyError(string mensagem)
        {
            Errors.Add(mensagem);
        }
    }
}
