using System.Threading.Tasks;
using BC7.Domain;

namespace BC7.Repository
{
    public interface IArticleRepository
    {
        Task CreateAsync(Article article);
    }
}