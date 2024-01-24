using Microsoft.OpenApi.Models;

namespace AgendaLarAPI.Configurations
{
    public static class SwaggerConfigurations
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Agenda Lar API",
                    Version = "v1",
                    Description = "Back-End API da Agenda Lar",
                    Contact = new OpenApiContact
                    {
                        Name = "Daniele Tavares",
                        Email = "danitavares.dev@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/danitavareslobo")
                    }
                });
            });

            return services;
        }

        public static WebApplication UseApiSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
