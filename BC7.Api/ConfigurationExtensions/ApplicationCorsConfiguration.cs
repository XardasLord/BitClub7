using System;
using System.Collections.Generic;
using BC7.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BC7.Api.ConfigurationExtensions
{
    public static class ApplicationCorsConfiguration
    {
        public static IServiceCollection ConfigureApplicationCors(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = new List<string>();

            var webUrl = configuration.GetSection("ApplicationSettings").GetSection("WebUrl").Value;
            var allowedOriginsConfig = configuration.GetSection("ApplicationSettings").GetSection("AllowedOrigins").Value;


            if (!webUrl.IsNullOrWhiteSpace())
            {
                allowedOrigins.Add(webUrl);
            }
            if (!allowedOriginsConfig.IsNullOrWhiteSpace())
            {
                var additionalOrigins = allowedOriginsConfig.Split(",", StringSplitOptions.RemoveEmptyEntries);
                allowedOrigins.AddRange(additionalOrigins);
            }

            services.AddCors(options =>
            {
                options.AddPolicy("BitClub7Policy",
                    builder =>
                    {
                        builder
                            .WithOrigins(allowedOrigins.ToArray())
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            return services;
        }
    }
}
