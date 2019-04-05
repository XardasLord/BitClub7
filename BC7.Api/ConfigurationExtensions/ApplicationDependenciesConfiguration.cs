using BC7.Business.Helpers;
using BC7.Business.Implementation.Helpers;
using BC7.Database;
using BC7.Repository;
using BC7.Repository.Implementation;
using BC7.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BC7.Api.ConfigurationExtensions
{
    public static class ApplicationDependenciesConfiguration
    {
        public static IServiceCollection ConfigureApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IReflinkHelper, ReflinkHelper>();
            services.AddTransient<IUserMultiAccountHelper, UserMultiAccountHelper>();
            services.AddTransient<IUserAccountDataHelper, UserAccountDataHelper>();
            services.AddTransient<IMatrixPositionHelper, MatrixPositionHelper>();

            services.AddTransient<IUserAccountDataRepository, UserAccountDataRepository>();
            
            services.AddDbContext<IBitClub7Context, BitClub7Context>(
                opts => opts.UseSqlServer(configuration.GetConnectionString("BitClub7DB"),
                    b => b.MigrationsAssembly(typeof(IBitClub7Context).Namespace))
            );

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            return services;
        }
    }
}
