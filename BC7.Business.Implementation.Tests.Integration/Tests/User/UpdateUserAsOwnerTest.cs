using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Business.Implementation.Users.Commands.UpdateUser;
using BC7.Security;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.User
{
    [Story(
        AsA = "As a owner",
        IWant = "I want to update my user data",
        SoThat = "So my data is updated"
    )]
    public class UpdateUserAsOwnerTest : BaseIntegration
    {
        private UpdateUserCommandHandler _sut;
        private UpdateUserCommand _command;
        private readonly Guid _updatedUserId = Guid.NewGuid();
        private readonly Guid _requestedUserId = Guid.NewGuid();

        void GivenSystemUnderTest()
        {
            _sut = new UpdateUserCommandHandler(_userAccountDataRepository);
        }

        async Task AndGivenCreatedUserAccountInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var user = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Id, _updatedUserId)
                .Generate();

            _context.UserAccountsData.Add(user);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _command = new UpdateUserCommand()
            {
                UserId = _updatedUserId,
                RequestedUserId = _updatedUserId,
                FirstName = "Updated first name",
                LastName = "Updated last name",
                Street = "Updated street",
                City = "Updated city",
                Country = "Updated country",
                ZipCode = "Updated zip code",
                BtcWalletAddress = "Updated btc wallet address",
                Role = UserRolesHelper.User
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
            user.Role.Should().Be(UserRolesHelper.User);
        }

        [Test]
        public void UpdateUser()
        {
            this.BDDfy();
        }
    }
}
