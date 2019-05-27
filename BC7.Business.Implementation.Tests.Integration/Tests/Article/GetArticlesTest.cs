using System;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Implementation.Articles.Requests;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using FluentAssertions;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.Article
{
    [Story(
        AsA = "As a user",
        IWant = "I want to get all articles",
        SoThat = "So all articles are returned"
    )]
    public class GetArticlesTest : BaseIntegration
    {
        private GetArticlesRequest _request;
        private GetArticlesRequestHandler _sut;
        private GetArticlesViewModel _result;
        private readonly Guid _userId = Guid.NewGuid();

        void GivenSystemUnderTest()
        {
            _sut = new GetArticlesRequestHandler(_mapper, _articleRepository);
        }

        async Task AndGivenCreatedArticlesInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var user = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Id, _userId)
                .RuleFor(x => x.FirstName, "John")
                .RuleFor(x => x.LastName, "Doo")
                .Generate();

            _context.UserAccountsData.Add(user);

            var articles = fakerGenerator.GetArticleFakerGenerator()
                .RuleFor(x => x.CreatorId, _userId)
                .Generate(5);

            _context.Articles.AddRange(articles);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _request = new GetArticlesRequest();
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
