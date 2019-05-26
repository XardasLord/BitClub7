using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Articles.Commands;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Domain;
using BC7.Security;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.CreateArticle
{
    [Story(
        AsA = "As a user with root role",
        IWant = "I want to create an article",
        SoThat = "So the new article is created"
    )]
    public class CreateArticleTest : BaseIntegration
    {
        private CreateArticleCommand _command;
        private CreateArticleCommandHandler _sut;
        private readonly Guid _userId = Guid.NewGuid();

        void GivenSystemUnderTest()
        {
            _sut = new CreateArticleCommandHandler(_userAccountDataRepository, _articleRepository);
        }

        async Task AndGivenCreatedRootUserInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var user = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Id, _userId)
                .RuleFor(x => x.Role, UserRolesHelper.Root)
                .Generate();

            _context.UserAccountsData.Add(user);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _command = new CreateArticleCommand
            {
                UserAccountDataId = _userId,
                Title = "Example Title",
                Text = "Example Text"
            };
        }

        async Task WhenHandlerHandlesTheCommand()
        {
            await _sut.Handle(_command);
        }

        async Task ThenArticleExistsInDatabase()
        {
            var article = await _context.Set<Article>().SingleAsync();
            
            article.CreatorId.Should().Be(_userId);
            article.Title.Should().Be("Example Title");
            article.Text.Should().Be("Example Text");
        }

        [Test]
        public void CreateNewArticle()
        {
            this.BDDfy();
        }
    }
}
