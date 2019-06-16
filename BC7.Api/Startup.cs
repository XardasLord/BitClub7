using System.Reflection;
using AutoMapper;
using BC7.Api.ConfigurationExtensions;
using BC7.Business.Implementation.Users.Commands.RegisterNewUserAccount;
using BC7.Business.Validators;
using BC7.Infrastructure.Implementation.RequestPipelines;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace BC7.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureApplicationDependencies(Configuration);
            services.ConfigureApplicationJwtAuthorization(Configuration);
            services.ConfigureApplicationCors(Configuration);

            services.AddAutoMapper();

            // Mediator
            services.AddMediatR(typeof(RegisterNewUserAccountCommand).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(RequestPreProcessorLogger<>));

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterNewUserModelValidator>());

            services.ConfigureSwaggerUI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UpdateDatabase();
            app.ConfigureCustomExceptionMiddleware();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseCors("BitClub7Policy");
            app.UseMvc();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BitClub7 API");

                c.DocumentTitle = "BitClub7 - API documentation";
                c.DocExpansion(DocExpansion.None);
            });
        }
    }
}
