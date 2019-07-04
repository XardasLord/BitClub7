using System.Collections.Generic;
using BC7.Database;
using BC7.Infrastructure.Implementation.ErrorHandling;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Dashboard.BasicAuthorization;
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

        public static IApplicationBuilder ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            return app;
        }

        public static IApplicationBuilder UseHangfire(this IApplicationBuilder app)
        {
            var filter = new BasicAuthAuthorizationFilter(
                new BasicAuthAuthorizationFilterOptions
                {
                    RequireSsl = false,
                    LoginCaseSensitive = true,
                    Users = new[]
                    {
                        new BasicAuthAuthorizationUser
                        {
                            Login = "dashboard",
                            PasswordClear = "Test$123"
                        }
                    }
                });
            
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new List<IDashboardAuthorizationFilter>()
                //Authorization = new[] { filter } // Basic auth with logic/password defined
                //Authorization = new[] { new HangfireAuthorizationFilter() } // Login in the future maybe with httpContext authorization, based on role, etc.
            });

            return app;
        }
    }
}
