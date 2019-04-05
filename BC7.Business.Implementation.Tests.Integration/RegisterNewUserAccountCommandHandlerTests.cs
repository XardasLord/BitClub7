using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Users.Commands.RegisterNewUserAccount;
using BC7.Entity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace BC7.Business.Implementation.Tests.Integration
{
    [TestFixture]
    public class RegisterNewUserAccountCommandHandlerTests : BaseIntegration
    {
        private RegisterNewUserAccountCommandHandler _sut;

        [Test]
        public async Task RegisterNewUserAccountCommandHandler_WhenHandleWithoutReflinkOrLogin_CreateUserAccountAndItsMultiAccountToRandomUser()
        {
            // Arrange
            _sut = new RegisterNewUserAccountCommandHandler(_context, _mapper, _userMultiAccountHelper, _userMultiAccountRepository);
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
            _sut = new RegisterNewUserAccountCommandHandler(_context, _mapper, _userMultiAccountHelper, _userMultiAccountRepository);
            await CreateExistingUsersInDatabase();
            var command = CreateCommand();
            command.InvitingUserLogin = "222";

            // Act
            var result = await _sut.Handle(command);

            // Assert
            var user = await _context.UserAccountsData
                .SingleOrDefaultAsync(x => x.Id == result);
            var multiAccount = await _context.UserMultiAccounts
                .Include(x => x.UserMultiAccountInviting)
                .SingleOrDefaultAsync(x => x.UserAccountDataId == result);

            user.Should().NotBeNull();
            multiAccount.Should().NotBeNull();
            multiAccount.IsMainAccount.Should().BeTrue();
            multiAccount.UserMultiAccountInviting.MultiAccountName.Should().Be("222");
        }

        [Test]
        public async Task RegisterNewUserAccountCommandHandler_WhenHandleWithInvitingReflink_CreateUserAccountAndItsMultiAccountToInvitingReflink()
        {
            // Arrange
            _sut = new RegisterNewUserAccountCommandHandler(_context, _mapper, _userMultiAccountHelper, _userMultiAccountRepository);
            await CreateExistingUsersInDatabase();
            var command = CreateCommand();
            command.InvitingRefLink = "reflink333";

            // Act
            var result = await _sut.Handle(command);

            // Assert
            var user = await _context.UserAccountsData
                .SingleOrDefaultAsync(x => x.Id == result);
            var multiAccount = await _context.UserMultiAccounts
                .Include(x => x.UserMultiAccountInviting)
                .SingleOrDefaultAsync(x => x.UserAccountDataId == result);

            user.Should().NotBeNull();
            multiAccount.Should().NotBeNull();
            multiAccount.IsMainAccount.Should().BeTrue();
            multiAccount.UserMultiAccountInviting.MultiAccountName.Should().Be("333");
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
            {
                Id = Guid.NewGuid(),
                Login = "ExistingLogin",
                Email = "Email",
                Salt = "salt",
                Hash = "hash",
                FirstName = "FirstName",
                LastName = "LastName",
                Street = "Street",
                City = "City",
                Country = "Country",
                ZipCode = "ZipCode",
                BtcWalletAddress = "BtcWalletAddress",
                Role = "Admin"
            };
            _context.UserAccountsData.Add(existingUserAccountData);

            await _context.SaveChangesAsync();

            _context.UserMultiAccounts.AddRange(
                new UserMultiAccount
                {
                    UserAccountDataId = existingUserAccountData.Id,
                    MultiAccountName = "111",
                    RefLink = "reflink111"
                },
                new UserMultiAccount
                {
                    UserAccountDataId = existingUserAccountData.Id,
                    MultiAccountName = "222",
                    RefLink = "reflink222"
                },
                new UserMultiAccount
                {
                    UserAccountDataId = existingUserAccountData.Id,
                    MultiAccountName = "333",
                    RefLink = "reflink333"
                });

            await _context.SaveChangesAsync();
        }
    }
}
