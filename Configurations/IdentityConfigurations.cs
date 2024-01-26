using AgendaLarAPI.Data;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;

namespace AgendaLarAPI.Configurations
{
    public static class IdentityConfigurations
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            if (appSettings is null)
                throw new ArgumentNullException(nameof(AppSettings));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(appSettings.ConnectionString);
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddErrorDescriber<IdentityPortugueseMessages>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = appSettings.Issuer,
                    ValidAudience = appSettings.ValidIn,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret))
                };
            });

            return services;
        }
    }
}
