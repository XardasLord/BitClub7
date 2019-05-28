using System;
using MediatR;

namespace BC7.Business.Implementation.Articles.Commands.DeleteArticle
{
    public class DeleteArticleCommand : IRequest
    {
        public Guid ArticleId { get; set; }
    }
}
