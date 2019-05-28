using System.Threading;
using System.Threading.Tasks;
using BC7.Repository;
using MediatR;

namespace BC7.Business.Implementation.Articles.Commands.DeleteArticle
{
    public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
    {
        private readonly IArticleRepository _articleRepository;

        public DeleteArticleCommandHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<Unit> Handle(DeleteArticleCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _articleRepository.DeleteAsync(command.ArticleId);

            return Unit.Value;
        }
    }
}
