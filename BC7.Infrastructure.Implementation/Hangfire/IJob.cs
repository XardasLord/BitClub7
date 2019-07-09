using System.Threading.Tasks;
using Hangfire.Server;

namespace BC7.Infrastructure.Implementation.Hangfire
{
    public interface IJob<in T>
    {
        Task Execute(T item, PerformContext context);
    }
}
