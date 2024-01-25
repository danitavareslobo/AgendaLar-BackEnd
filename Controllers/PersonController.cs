using AgendaLarAPI.Controllers.Base;
using AgendaLarAPI.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

using System.Text;

namespace AgendaLarAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PersonController : DefaultController
    {
        public PersonController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CustomResponse(await);
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