using AgendaLarAPI.Controllers.Base;
using AgendaLarAPI.Models.User.ViewModels;
using AgendaLarAPI.Services;
using AgendaLarAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgendaLarAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : DefaultController
    {
        private readonly IAuthService _service;

        public AuthController(
            IAuthService service,
            NotificationService notificationService)
            : base(notificationService)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegister user)
        {
            return CustomResponse(await _service.RegisterUserAsync(user));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLogin user)
        {
            return CustomResponse(await _service.Login(user));
        }
    }
}
