using System;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Implementation.Articles.Requests.GetArticles;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.Article
{
    [Story(
        AsA = "As a user",
        IWant = "I want to get all standard articles",
        SoThat = "So all standard articles are returned"
    )]
    public class GetArticlesTest : BaseIntegration
    {
        private GetArticlesRequest _request;
        private GetArticlesRequestHandler _sut;
        private GetArticlesViewModel _result;
        private readonly Guid _userId = Guid.NewGuid();

        void GivenSystemUnderTest()
        {
            _sut = new GetArticlesRequestHandler(_mapper, _articleRepository, _userAccountDataRepository);
        }

        async Task AndGivenCreatedArticlesInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var user = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Id, _userId)
                .RuleFor(x => x.FirstName, "John")
                .RuleFor(x => x.LastName, "Doo")
                .Generate();

            var otherUser = fakerGenerator.GetUserAccountDataFakerGenerator().Generate();

            _context.UserAccountsData.Add(user);
            _context.UserAccountsData.Add(otherUser);

            var standardArticles = fakerGenerator.GetArticleFakerGenerator()
                .RuleFor(x => x.CreatorId, _userId)
                .RuleFor(x => x.ArticleType, ArticleType.Standard)
                .Generate(5);

            var cryptoBlogArticles = fakerGenerator.GetArticleFakerGenerator()
                .RuleFor(x => x.CreatorId, otherUser.Id)
                .RuleFor(x => x.ArticleType, ArticleType.CryptoBlog)
                .Generate(3);

            _context.Articles.AddRange(standardArticles);
            _context.Articles.AddRange(cryptoBlogArticles);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _request = new GetArticlesRequest { ArticleType = ArticleType.Standard };
        }

        async Task WhenHandlerHandlesTheCommand()
        {
            _result = await _sut.Handle(_request);
        }

        void ThenResultHasAllArticlesFromDatabase()
        {
            _result.Articles.Count().Should().Be(5);
            _result.Articles.All(x => x.Creator == "John Doo").Should().BeTrue();
        }

        [Test]
        public void GetAllArticles()
        {
            this.BDDfy();
        }
    }
}
