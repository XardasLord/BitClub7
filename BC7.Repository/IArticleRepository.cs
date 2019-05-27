using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Domain;

namespace BC7.Repository
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAllAsync();
        Task CreateAsync(Article article);
    }
}