using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Models;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Articles.Requests.GetArticles
{
    public class GetArticlesRequestHandler : IRequestHandler<GetArticlesRequest, GetArticlesViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IArticleRepository _articleRepository;

        public GetArticlesRequestHandler(IMapper mapper, IArticleRepository articleRepository)
        {
            _mapper = mapper;
            _articleRepository = articleRepository;
        }

        public async Task<GetArticlesViewModel> Handle(GetArticlesRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var articles = await _articleRepository.GetAllAsync();

            var articleModels = _mapper.Map<IEnumerable<ArticleModel>>(articles);

            return new GetArticlesViewModel(articleModels);
        }
    }
}
