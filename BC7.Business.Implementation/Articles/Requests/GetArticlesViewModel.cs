using System.Collections.Generic;
using BC7.Business.Models;

namespace BC7.Business.Implementation.Articles.Requests
{
    public class GetArticlesViewModel
    {
        public IEnumerable<ArticleModel> Articles { get; }

        public GetArticlesViewModel(IEnumerable<ArticleModel> articles)
        {
            Articles = articles;
        }
    }
}
