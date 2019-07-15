using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Models;
using BC7.Domain.Enums;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Articles.Requests.GetArticles
{
    public class GetArticlesRequestHandler : IRequestHandler<GetArticlesRequest, GetArticlesViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IArticleRepository _articleRepository;
        private readonly IUserAccountDataRepository _userAccountDataRepository;

        public GetArticlesRequestHandler(IMapper mapper, IArticleRepository articleRepository, IUserAccountDataRepository userAccountDataRepository)
        {
            _mapper = mapper;
            _articleRepository = articleRepository;
            _userAccountDataRepository = userAccountDataRepository;
        }

        public async Task<GetArticlesViewModel> Handle(GetArticlesRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (request.ArticleType == ArticleType.CryptoBlog)
            {
                var user = await _userAccountDataRepository.GetAsync(request.RequestedUser.Id);
                if (!user.IsMembershipFeePaid)
                {
                    throw new ValidationException("You have to pay membership's fee to get cryptoblog articles");
                }
            }

            var articles = await _articleRepository.GetAllByStatusAsync(request.ArticleType);

            var articleModels = _mapper.Map<IEnumerable<ArticleModel>>(articles);

            return new GetArticlesViewModel(articleModels);
        }
    }
}
