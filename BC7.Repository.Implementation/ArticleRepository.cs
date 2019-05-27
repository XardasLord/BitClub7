using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC7.Database;
using BC7.Domain;
using Microsoft.EntityFrameworkCore;

namespace BC7.Repository.Implementation
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IBitClub7Context _context;

        public ArticleRepository(IBitClub7Context context)
        {
            _context = context;
        }

        public Task<Article> GetAsync(Guid id)
        {
            return _context.Set<Article>().SingleAsync(x => x.Id == id);
        }

        public Task<List<Article>> GetAllAsync()
        {
            return _context.Set<Article>()
                .Include(x => x.Creator)
                .ToListAsync();
        }

        public Task CreateAsync(Article article)
        {
            _context.Set<Article>().Add(article);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Article article)
        {
            _context.Set<Article>().Attach(article);
            return _context.SaveChangesAsync();
        }
    }
}