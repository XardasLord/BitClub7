using System.Threading.Tasks;
using BC7.Database;
using BC7.Domain;

namespace BC7.Repository.Implementation
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IBitClub7Context _context;

        public ArticleRepository(IBitClub7Context context)
        {
            _context = context;
        }

        public Task CreateAsync(Article article)
        {
            _context.Set<Article>().Add(article);
            return _context.SaveChangesAsync();
        }
    }
}