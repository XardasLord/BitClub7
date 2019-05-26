using System;
using MediatR;

namespace BC7.Business.Implementation.Articles.Commands
{
    public class CreateArticleCommand : IRequest
    {
        public Guid UserAccountDataId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
