using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using BC7.Api.ConfigurationExtensions;
using BC7.Business.Helpers;
using BC7.Business.Implementation.Helpers;
using BC7.Business.Implementation.Users.Commands.RegisterNewUserAccount;
using BC7.Business.Validators;
using BC7.Database;
using BC7.Security;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace BC7.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: Move to extension
            services.AddTransient<IReflinkHelper, ReflinkHelper>();
            services.AddTransient<IUserMultiAccountHelper, UserMultiAccountHelper>();
            services.AddDbContext<IBitClub7Context, BitClub7Context>(
                opts => opts.UseSqlServer(Configuration.GetConnectionString("BitClub7DB"),
                    b => b.MigrationsAssembly(typeof(IBitClub7Context).Namespace))
            );

            services.ConfigureApplicationJwtAuthorization(Configuration);

            services.AddAutoMapper();
            services.AddMediatR(typeof(RegisterNewUserAccountCommand).Assembly); // TODO: Getting Assembly can be done more universal and pretty

            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterNewUserModelValidator>()); // TODO: Getting Assembly can be done more universal and pretty

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1 alpha",
                    Title = "BitClub7",
                    Description = "BitClub7 API",
                    TermsOfService = "None",
                    //Contact = new Contact() { Name = "Talking Dotnet", Email = "contact@talkingdotnet.com", Url = "www.talkingdotnet.com" }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UpdateDatabase();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BitClub7 API");
            });
        }
    }
}
