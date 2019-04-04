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
        public async Task RegisterNewUserAccountCommandHandler_WhenHandle_CreateUserAccountAndItsMultiAccount()
        {
            // Arrange
            await CreateTestUserInDatabase();
            _sut = new RegisterNewUserAccountCommandHandler(_context, _mapper, _reflinkHelper, _userMultiAccountHelper);
            var command = new RegisterNewUserAccountCommand
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
                BtcWalletAddress = "5JxwqzhrZBSDMu7BBzDaK2rMhL8PwUniikA45aJ9JutaBKS3iyS"
            };

            // Act
            var result = await _sut.Handle(command);

            // Assert
            var user = await _context.UserAccountsData.SingleOrDefaultAsync(x => x.Id == result);
            var multiAccount = await _context.UserMultiAccounts.SingleOrDefaultAsync(x => x.UserAccountDataId == result);
            user.Should().NotBeNull();
            multiAccount.Should().NotBeNull();
            multiAccount.IsMainAccount.Should().BeTrue();
        }

        private async Task CreateTestUserInDatabase()
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

            _context.UserMultiAccounts.Add(new UserMultiAccount
            {
                UserAccountDataId = existingUserAccountData.Id
            });

            await _context.SaveChangesAsync();
        }
    }
}
