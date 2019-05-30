using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BC7.Business.Models;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Articles.Requests.GetArticle
{
    public class GetArticleRequestHandler : IRequestHandler<GetArticleRequest, ArticleModel>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;

        public GetArticleRequestHandler(IArticleRepository articleRepository, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
        }

        public async Task<ArticleModel> Handle(GetArticleRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var article = await _articleRepository.GetAsync(request.Id);

            return _mapper.Map<ArticleModel>(article);
        }
    }
}
