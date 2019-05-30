using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Business.Implementation.Users.Commands.UpdateUser;
using BC7.Business.Models;
using BC7.Security;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.User
{
    [Story(
        AsA = "As a root",
        IWant = "I want to update other user data",
        SoThat = "So the other`s user data is updated"
    )]
    public class UpdateOtherUserAsRootTest : BaseIntegration
    {
        private UpdateUserCommandHandler _sut;
        private UpdateUserCommand _command;
        private readonly Guid _updatedUserId = Guid.NewGuid();
        private readonly Guid _requestedUserId = Guid.NewGuid();
        private readonly string _requestedUserEmail = "test@email.com";
        private readonly string _requestedUserRole = UserRolesHelper.Root;

        void GivenSystemUnderTest()
        {
            _sut = new UpdateUserCommandHandler(_userAccountDataRepository);
        }

        async Task AndGivenCreatedUserAccountInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var user = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Id, _updatedUserId)
                .RuleFor(x => x.Role, UserRolesHelper.User)
                .Generate();

            var root = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Id, _requestedUserId)
                .RuleFor(x => x.Email, _requestedUserEmail)
                .RuleFor(x => x.Role, _requestedUserRole)
                .Generate();

            _context.UserAccountsData.AddRange(user, root);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _command = new UpdateUserCommand()
            {
                UserId = _updatedUserId,
                RequestedUser = new LoggedUserModel(_requestedUserId, _requestedUserEmail, _requestedUserRole),
                FirstName = "Updated first name",
                LastName = "Updated last name",
                Street = "Updated street",
                City = "Updated city",
                Country = "Updated country",
                ZipCode = "Updated zip code",
                BtcWalletAddress = "Updated btc wallet address",
                Role = UserRolesHelper.Admin
            };
        }

        async Task WhenHandlerHandlesTheCommand()
        {
            await _sut.Handle(_command);
        }

        async Task ThenUserIsUpdatedInDatabase()
        {
            var user = await _context.UserAccountsData.SingleAsync(x => x.Id == _updatedUserId);

            user.FirstName.Should().Be("Updated first name");
            user.LastName.Should().Be("Updated last name");
            user.Street.Should().Be("Updated street");
            user.City.Should().Be("Updated city");
            user.Country.Should().Be("Updated country");
            user.ZipCode.Should().Be("Updated zip code");
            user.BtcWalletAddress.Should().Be("Updated btc wallet address");
            user.Role.Should().Be(UserRolesHelper.Admin);
        }

        [Test]
        public void UpdateOtherUserAsRoot()
        {
            this.BDDfy();
        }
    }
}
