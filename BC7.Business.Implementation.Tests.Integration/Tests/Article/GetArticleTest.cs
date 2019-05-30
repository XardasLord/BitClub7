using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Articles.Requests.GetArticle;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Business.Models;
using FluentAssertions;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.Article
{
    [Story(
        AsA = "As a user",
        IWant = "I want to get a specific article by ID",
        SoThat = "So the requested article is returned"
    )]
    public class GetArticleTest : BaseIntegration
    {
        private GetArticleRequest _request;
        private GetArticleRequestHandler _sut;
        private ArticleModel _result;
        private readonly Guid _articleId = Guid.NewGuid();

        void GivenSystemUnderTest()
        {
            _sut = new GetArticleRequestHandler(_articleRepository, _mapper);
        }

        async Task AndGivenCreatedArticlesInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var user = fakerGenerator.GetUserAccountDataFakerGenerator().Generate();
            _context.UserAccountsData.Add(user);

            var testedArticle = fakerGenerator.GetArticleFakerGenerator()
                .RuleFor(x => x.Id, _articleId)
                .RuleFor(x => x.CreatorId, user.Id)
                .Generate();

            var articles = fakerGenerator.GetArticleFakerGenerator()
                .RuleFor(x => x.CreatorId, user.Id)
                .Generate(5);

            articles.Add(testedArticle);

            _context.Articles.AddRange(articles);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _request = new GetArticleRequest
            {
                Id = _articleId
            };
        }

        async Task WhenHandlerHandlesTheCommand()
        {
            _result = await _sut.Handle(_request);
        }

        void ThenResultIsRequestedArticleFromDatabase()
        {
            _result.Id.Should().Be(_articleId);
        }

        [Test]
        public void GetArticleById()
        {
            this.BDDfy();
        }
    }
}
