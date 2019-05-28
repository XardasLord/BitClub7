using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Articles.Commands.DeleteArticle;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.Article
{
    [Story(
        AsA = "As a user with root role",
        IWant = "I want to delete an existing article from database",
        SoThat = "So the article is deleted"
    )]
    public class DeleteArticleTest : BaseIntegration
    {
        private DeleteArticleCommand _command;
        private DeleteArticleCommandHandler _sut;
        private readonly Guid _articleId = Guid.NewGuid();

        void GivenSystemUnderTest()
        {
            _sut = new DeleteArticleCommandHandler(_articleRepository);
        }

        async Task AndGivenCreatedArticleInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var user = fakerGenerator.GetUserAccountDataFakerGenerator().Generate();

            _context.UserAccountsData.Add(user);

            var article = fakerGenerator.GetArticleFakerGenerator()
                .RuleFor(x => x.Id, _articleId)
                .RuleFor(x => x.CreatorId, user.Id)
                .Generate();

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _command = new DeleteArticleCommand
            {
                ArticleId = _articleId
            };
        }

        async Task WhenHandlerHandlesTheCommand()
        {
            await _sut.Handle(_command);
        }

        async Task ThenArticleIsDeletedFromDatabase()
        {
            var article = await _context.Articles.CountAsync();

            article.Should().Be(0);
        }

        [Test]
        public void DeleteArticle()
        {
            this.BDDfy();
        }
    }
}
