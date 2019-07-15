using BC7.Business.Models;
using BC7.Domain.Enums;
using MediatR;

namespace BC7.Business.Implementation.Articles.Requests.GetArticles
{
    public class GetArticlesRequest : IRequest<GetArticlesViewModel>
    {
        public LoggedUserModel RequestedUser { get; set; }
        public ArticleType ArticleType { get; set; }
    }
}
