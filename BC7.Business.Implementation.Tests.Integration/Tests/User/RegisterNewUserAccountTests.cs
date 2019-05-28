using System.Threading.Tasks;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Business.Implementation.Users.Commands.RegisterNewUserAccount;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace BC7.Business.Implementation.Tests.Integration.Tests.User
{
    [TestFixture]
    public class RegisterNewUserAccountTests : BaseIntegration
    {
        private RegisterNewUserAccountCommandHandler _sut;

        [Test]
        public async Task RegisterNewUserAccountCommandHandler_WhenHandleWithoutReflinkOrLogin_CreateUserAccountAndItsMultiAccountToRandomUser()
        {
            // Arrange
            _sut = new RegisterNewUserAccountCommandHandler(_context, _userMultiAccountHelper, _userMultiAccountRepository);
            await CreateExistingUsersInDatabase();
            var command = CreateCommand();

            // Act
            var result = await _sut.Handle(command);

            // Assert
            var user = await _context.UserAccountsData.SingleOrDefaultAsync(x => x.Id == result);
            var multiAccount = await _context.UserMultiAccounts.SingleOrDefaultAsync(x => x.UserAccountDataId == result);
            user.Should().NotBeNull();
            multiAccount.Should().NotBeNull();
            multiAccount.IsMainAccount.Should().BeTrue();
        }

        [Test]
        public async Task RegisterNewUserAccountCommandHandler_WhenHandleWithInvitingLogin_CreateUserAccountAndItsMultiAccountToInvitingUser()
        {
            // Arrange
            _sut = new RegisterNewUserAccountCommandHandler(_context, _userMultiAccountHelper, _userMultiAccountRepository);
            await CreateExistingUsersInDatabase();
            var command = CreateCommand();
            command.SponsorLogin = "222";

            // Act
            var result = await _sut.Handle(command);

            // Assert
            var user = await _context.UserAccountsData
                .SingleOrDefaultAsync(x => x.Id == result);
            var multiAccount = await _context.UserMultiAccounts
                .Include(x => x.Sponsor)
                .SingleOrDefaultAsync(x => x.UserAccountDataId == result);

            user.Should().NotBeNull();
            multiAccount.Should().NotBeNull();
            multiAccount.IsMainAccount.Should().BeTrue();
            multiAccount.Sponsor.MultiAccountName.Should().Be("222");
        }

        [Test]
        public async Task RegisterNewUserAccountCommandHandler_WhenHandleWithInvitingReflink_CreateUserAccountAndItsMultiAccountToInvitingReflink()
        {
            // Arrange
            _sut = new RegisterNewUserAccountCommandHandler(_context, _userMultiAccountHelper, _userMultiAccountRepository);
            await CreateExistingUsersInDatabase();
            var command = CreateCommand();
            command.SponsorRefLink = "reflink333";

            // Act
            var result = await _sut.Handle(command);

            // Assert
            var user = await _context.UserAccountsData
                .SingleOrDefaultAsync(x => x.Id == result);
            var multiAccount = await _context.UserMultiAccounts
                .Include(x => x.Sponsor)
                .SingleOrDefaultAsync(x => x.UserAccountDataId == result);

            user.Should().NotBeNull();
            multiAccount.Should().NotBeNull();
            multiAccount.IsMainAccount.Should().BeTrue();
            multiAccount.Sponsor.MultiAccountName.Should().Be("333");
        }

        private static RegisterNewUserAccountCommand CreateCommand()
        {
            return new RegisterNewUserAccountCommand
            {
                Login = "Login123",
                Email = "Email@test.pl",
                Password = "Password123456",
                FirstName = "Jan",
                LastName = "Nowak",
                Street = "Odrodzenia 12",
                City = "Gdańsk",
                Country = "Polska",
                ZipCode = "01-234",
                BtcWalletAddress = "5JxwqzhrZBSDMu7BBzDaK2rMhL8PwUniikA45aJ9JutaBKS3iyS",
                SponsorRefLink = null,
                SponsorLogin = null
            };
        }

        private async Task CreateExistingUsersInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var existingUserAccountData = fakerGenerator.GetUserAccountDataFakerGenerator().Generate();

            _context.UserAccountsData.Add(existingUserAccountData);
            await _context.SaveChangesAsync();

            var multiAccount1 = fakerGenerator.GetUserMultiAccountFakerGenerator()
                .RuleFor(x => x.UserAccountDataId, existingUserAccountData.Id)
                .Generate();

            var multiAccount2 = fakerGenerator.GetUserMultiAccountFakerGenerator()
                .RuleFor(x => x.UserAccountDataId, existingUserAccountData.Id)
                .RuleFor(x => x.MultiAccountName, "222")
                .Generate();

            var multiAccount3 = fakerGenerator.GetUserMultiAccountFakerGenerator()
                .RuleFor(x => x.UserAccountDataId, existingUserAccountData.Id)
                .RuleFor(x => x.MultiAccountName, "333")
                .RuleFor(x => x.RefLink, "reflink333")
                .Generate();

            _context.UserMultiAccounts.AddRange(multiAccount1, multiAccount2, multiAccount3);
            await _context.SaveChangesAsync();
        }
    }
}
