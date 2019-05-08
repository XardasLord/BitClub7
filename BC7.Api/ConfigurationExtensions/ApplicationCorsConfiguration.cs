using Microsoft.Extensions.DependencyInjection;

namespace BC7.Api.ConfigurationExtensions
{
    public static class ApplicationCorsConfiguration
    {
        public static IServiceCollection ConfigureApplicationCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            return services;
        }
    }
}
