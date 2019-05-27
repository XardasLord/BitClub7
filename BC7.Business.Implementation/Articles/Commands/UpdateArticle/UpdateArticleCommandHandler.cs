using System.Threading;
using System.Threading.Tasks;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Articles.Commands.UpdateArticle
{
    public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand>
    {
        private readonly IArticleRepository _articleRepository;

        public UpdateArticleCommandHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<Unit> Handle(UpdateArticleCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var article = await _articleRepository.GetAsync(command.ArticleId);

            article.UpdateInformation(command.Title, command.Text);

            await _articleRepository.UpdateAsync(article);

            return Unit.Value;
        }
    }
}