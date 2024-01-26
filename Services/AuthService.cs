using AgendaLarAPI.Configurations;
using AgendaLarAPI.Models.Notification;
using AgendaLarAPI.Models.User;
using AgendaLarAPI.Models;
using AgendaLarAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgendaLarAPI.Extensions;

namespace AgendaLarAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IPersonService _personService;
        private readonly NotificationService _notificationService;
        private readonly AppSettings _appSettings;

        public AuthService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IPersonService personService,
            NotificationService notificationService,
            IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _personService = personService;
            _notificationService = notificationService;
            _appSettings = appSettings.Value;
        }

        public async Task<UserLoginResponse?> RegisterUserAsync(UserRegister userRegister)
        {
            if (!userRegister.IsValid)
            {
                _notificationService.AddNotification("Usuário", "Todos os campos são obrigatórios");
                return null;
            }

            var user = new IdentityUser
            {
                UserName = userRegister.Email,
                Email = userRegister.Email,
                EmailConfirmed = true
            };

            var result = await CreateUserAsync(userRegister, user);

            if (result.Succeeded) return await GenerateJwt(userRegister.Email);

            var errors = result.Errors.Select(e => new Notification(e.Code, e.Description));

            _notificationService.AddNotifications(errors);

            return null;
        }

        public async Task<UserLoginResponse?> Login(UserLogin userLogin)
        {
            if (ValidateLogin(userLogin)) return null;

            var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, true);

            if (!result.Succeeded)
            {
                return ConfigureLoginErrorResponse(result);
            }

            var token = await GenerateJwt(userLogin.Email);

            if (token != null) return token;

            _notificationService.AddNotification(
                NotificationType.Error.GetEnumDescription(),
                "Falha ao gerar o token",
                NotificationType.Error);

            return null;
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

        private async Task<IdentityResult> CreateUserAsync(UserRegister userRegister, IdentityUser user)
        {
            var result = await _userManager.CreateAsync(user, userRegister.Password);

            if (!await _roleManager.RoleExistsAsync(AgendaConstants.AdminRole))
                await _roleManager.CreateAsync(new IdentityRole(AgendaConstants.AdminRole));

            await _userManager.AddToRoleAsync(user, AgendaConstants.AdminRole);
            await _personService.AddAsync(new Models.Person.Person
            {
                Name = userRegister.Name,
                Email = userRegister.Email,
                SocialNumber = userRegister.SocialNumber,
                BirthDate = DateTime.Now.AddYears(-18),
            });
            return result;
        }

        private UserLoginResponse? ConfigureLoginErrorResponse(SignInResult result)
        {
            if (result.IsLockedOut)
            {
                _notificationService.AddNotification("Bloqueado", "Usuário temporariamente bloqueado por tentativas inválidas");
                return null;
            }

            if (result.IsNotAllowed)
            {
                _notificationService.AddNotification(
                    NotificationType.Unauthorized.GetEnumDescription(),
                    "Usuário não autorizado",
                    NotificationType.Unauthorized);
                return null;
            }

            _notificationService.AddNotification("Usuário", "Usuário ou senha incorretos");

            return null;
        }

        private bool ValidateLogin(UserLogin userLogin)
        {
            if (userLogin.IsEmpty)
            {
                _notificationService.AddNotification("Login", "Usuário e senha são obrigatórios");
                return true;
            }

            if (!userLogin.IsValidEmail)
            {
                _notificationService.AddNotification("Email", "Email informado não é válido.");
                return true;
            }

            if (!userLogin.IsValidPassword)
            {
                _notificationService.AddNotification("Senha", "Senha informada não é válida.");
                return true;
            }

            return false;
        }
    }
}
