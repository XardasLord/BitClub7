﻿using BC7.Database;
using BC7.Infrastructure.Implementation.ErrorHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BC7.Api.ConfigurationExtensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Updates database when new migration is available. When database is not created it will create it.
        /// </summary>
        /// <param name="app">The Microsoft.AspNetCore.Builder.IApplicationBuilder.</param>
        /// <returns>A reference to the app after the operation has completed.</returns>
        public static IApplicationBuilder UpdateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetRequiredService<IBitClub7Context>())
                {
                    context.Database.Migrate();
                }
            }

            return app;
        }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
