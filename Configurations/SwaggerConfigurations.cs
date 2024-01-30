using Microsoft.Extensions.Options;
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

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        public static WebApplication UseApiSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(swaggerOptions =>
            {
                swaggerOptions.SwaggerEndpoint("swagger/v1/swagger.json", "Agenda Lar API");
                swaggerOptions.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}
