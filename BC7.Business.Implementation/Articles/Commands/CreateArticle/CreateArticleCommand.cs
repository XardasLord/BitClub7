using System;
using MediatR;

namespace BC7.Business.Implementation.Articles.Commands.CreateArticle
{
    public class CreateArticleCommand : IRequest<Guid>
    {
        public Guid UserAccountDataId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
