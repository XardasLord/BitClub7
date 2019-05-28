using System;
using MediatR;

namespace BC7.Business.Implementation.Articles.Commands.UpdateArticle
{
    public class UpdateArticleCommand : IRequest
    {
        public Guid ArticleId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
