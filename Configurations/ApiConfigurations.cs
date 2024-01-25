using AgendaLarAPI.Data.Repositories.Interfaces;
using AgendaLarAPI.Data.Repositories;
using AgendaLarAPI.Services.Interfaces;
using AgendaLarAPI.Services;

using System.Net;

namespace AgendaLarAPI.Configurations
{
    public static class ApiConfigurations
    {
        public static IServiceCollection ConfigureApi(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection(nameof(AppSettings));
            services.Configure<AppSettings>(appSettings);
            services.AddRouting(route => route.LowercaseUrls = true);
            services.AddControllers();
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = (context) =>
                {
                    context.ProblemDetails.Title = context.ProblemDetails.Status switch
                    {
                        (int)HttpStatusCode.BadRequest => "Ocorreu um ou mais erros de validação.",
                        (int)HttpStatusCode.Unauthorized => "Não autorizado.",
                        (int)HttpStatusCode.Forbidden => "Usuário sem permissão.",
                        (int)HttpStatusCode.NotFound => "O recurso solicitado não foi encontrado.",
                        (int)HttpStatusCode.InternalServerError => "Ocorreu um erro interno no servidor.",
                        _ => context.ProblemDetails.Title
                    };
                };
            });
            services.AddEndpointsApiExplorer();

            return services;
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<NotificationService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<IPersonService, PersonService>();
            return services;
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPhoneRepository, PhoneRepository>();
        }

        public static WebApplication UseApiConfiguration(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();
            app.MapControllers();

            return app;
        }
    }

}
