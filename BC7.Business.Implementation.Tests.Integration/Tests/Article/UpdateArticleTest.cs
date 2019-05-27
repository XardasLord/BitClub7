using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Articles.Commands.UpdateArticle;
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
        IWant = "I want to update an existing article",
        SoThat = "So the article is updated"
    )]
    public class UpdateArticleTest : BaseIntegration
    {
        private UpdateArticleCommand _command;
        private UpdateArticleCommandHandler _sut;
        private readonly Guid _articleId = Guid.NewGuid();

        void GivenSystemUnderTest()
        {
            _sut = new UpdateArticleCommandHandler(_articleRepository);
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
            _command = new UpdateArticleCommand
            {
                ArticleId = _articleId,
                Title = "New Title",
                Text = "New Text"
            };
        }

        async Task WhenHandlerHandlesTheCommand()
        {
            await _sut.Handle(_command);
        }

        async Task ThenArticleIsUpdatedInDatabase()
        {
            var article = await _context.Articles.SingleAsync(x => x.Id == _articleId);

            article.Title.Should().Be("New Title");
            article.Text.Should().Be("New Text");
        }

        [Test]
        public void UpdateArticle()
        {
            this.BDDfy();
        }
    }
}
