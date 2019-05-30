using System;
using BC7.Business.Models;
using MediatR;

namespace BC7.Business.Implementation.Articles.Requests.GetArticle
{
    public class GetArticleRequest : IRequest<ArticleModel>
    {
        public Guid Id { get; set; }
    }
}
