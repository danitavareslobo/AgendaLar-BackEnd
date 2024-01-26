using AgendaLarAPI.Configurations;
using AgendaLarAPI.Controllers.Base;
using AgendaLarAPI.Models;
using AgendaLarAPI.Models.User;
using AgendaLarAPI.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AgendaLarAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : DefaultController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppSettings _appSettings;

        public AuthController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            NotificationService notificationService,
            IOptions<AppSettings> appSettings)
            : base(notificationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegister userRegister)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState.Values.SelectMany(e => e.Errors));

            var user = new IdentityUser
            {
                UserName = userRegister.Email,
                Email = userRegister.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRegister.Password);

            if (!await _roleManager.RoleExistsAsync(AgendaConstants.AdminRole))
                await _roleManager.CreateAsync(new IdentityRole(AgendaConstants.AdminRole));

            await _userManager.AddToRoleAsync(user, AgendaConstants.AdminRole);

            if (result.Succeeded) return CustomResponse(await GenerateJwt(userRegister.Email));

            var errors = result.Errors.Select(e => e.Description);

            foreach (var error in errors)
                NotifyError(error);

            return CustomResponse();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    NotifyError("Usuário temporariamente bloqueado por tentativas inválidas");
                    return CustomResponse();
                }

                if (result.IsNotAllowed)
                {
                    NotifyError("Usuário não autorizado");
                    return CustomResponse();
                }

                NotifyError("Usuário ou senha incorretos");

                return CustomResponse();
            }

            var token = await GenerateJwt(userLogin.Email);

            if (token != null) return CustomResponse(token);

            NotifyError("Falha ao gerar o token");
            return CustomResponse();
        }

        private async Task<UserLoginResponse?> GenerateJwt(string email)
        {
            if (string.IsNullOrWhiteSpace(_appSettings.Secret)
                || string.IsNullOrWhiteSpace(_appSettings.Issuer)
                || string.IsNullOrWhiteSpace(_appSettings.ValidIn)) return null;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return null;

            var identityClaims = await GetIdentityClaims(user);
            var expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours);
            var encodedToken = GetEncodedToken(identityClaims, expires);

            return new UserLoginResponse
            {
                AccessToken = encodedToken,
                ExpiresIn = expires,
                UserToken = new UserToken
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Claims = identityClaims.Claims.Select(c => new UserClaim { Type = c.Type, Value = c.Value })
                }
            };
        }

        private string GetEncodedToken(ClaimsIdentity identityClaims, DateTime expires)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidIn,
                Subject = identityClaims,
                Expires = expires,
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private async Task<ClaimsIdentity> GetIdentityClaims(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            identityClaims.AddClaims(claims);

            foreach (var role in roles)
                identityClaims.AddClaim(new Claim("role", role));

            return identityClaims;
        }
    }
}
