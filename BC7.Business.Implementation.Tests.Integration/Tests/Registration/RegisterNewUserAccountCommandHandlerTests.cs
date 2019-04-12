using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Users.Commands.RegisterNewUserAccount;
using BC7.Domain;
using BC7.Security;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace BC7.Business.Implementation.Tests.Integration.Tests.Registration
{
    [TestFixture]
    public class RegisterNewUserAccountCommandHandlerTests : BaseIntegration
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
            command.InvitingUserLogin = "222";

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
            command.InvitingRefLink = "reflink333";

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
                InvitingRefLink = null,
                InvitingUserLogin = null
            };
        }

        private async Task CreateExistingUsersInDatabase()
        {
            var existingUserAccountData = new UserAccountData
            (
                id: Guid.NewGuid(),
                login: "ExistingLogin",
                email: "Email",
                firstName: "FirstName",
                lastName: "LastName",
                street: "Street",
                city: "City",
                country: "Country",
                zipCode: "ZipCode",
                btcWalletAddress: "BtcWalletAddress",
                role: UserRolesHelper.Admin
            );
            existingUserAccountData.SetPassword("salt", "hash");

            _context.UserAccountsData.Add(existingUserAccountData);

            await _context.SaveChangesAsync();

            var multiAccount1 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: existingUserAccountData.Id,
                sponsorId: null,
                multiAccountName: "111"
            );
            multiAccount1.SetReflink("reflink111");

            var multiAccount2 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: existingUserAccountData.Id,
                sponsorId: null,
                multiAccountName: "222"
            );
            multiAccount2.SetReflink("reflink222");

            var multiAccount3 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: existingUserAccountData.Id,
                sponsorId: null,
                multiAccountName: "333"
            );
            multiAccount3.SetReflink("reflink333");

            _context.UserMultiAccounts.AddRange(multiAccount1, multiAccount2, multiAccount3);

            await _context.SaveChangesAsync();
        }
    }
}
