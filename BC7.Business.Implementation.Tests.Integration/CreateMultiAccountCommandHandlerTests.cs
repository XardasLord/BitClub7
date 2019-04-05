using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Users.Commands.CreateMultiAccount;
using BC7.Entity;
using FluentAssertions;
using NUnit.Framework;

namespace BC7.Business.Implementation.Tests.Integration
{
    public class CreateMultiAccountCommandHandlerTests : BaseIntegration
    {
        private CreateMultiAccountCommandHandler _sut;

        [Test]
        public async Task CreateMultiAccountCommandHandler_WhenHandle_CreateMultiAccountForTheUser()
        {
            _sut = new CreateMultiAccountCommandHandler(_context, _userAccountDataRepository, _userMultiAccountRepository, _userMultiAccountHelper, _matrixPositionHelper);
            await CreateUserAndMultiAccountAndMatrixPositionsInDatabase();
            var command = new CreateMultiAccountCommand
            {
                UserAccountId = Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499"),
                RefLink = "otherUserReflink12345"
            };

            var result = await _sut.Handle(command);

            result.Should().NotBe(Guid.Empty);
        }

        private async Task CreateUserAndMultiAccountAndMatrixPositionsInDatabase()
        {
            var existingUserAccountData = new UserAccountData
            {
                Id = Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499"),
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
                Role = "User"
            };

            var otherUser = new UserAccountData
            {
                Id = Guid.NewGuid(),
                Login = "OtherLogin",
                Email = "OtherEmail",
                Salt = "OtherSalt",
                Hash = "OtherHash",
                FirstName = "OtherFirstName",
                LastName = "OtherLastName",
                Street = "OtherStreet",
                City = "OtherCity",
                Country = "OtherCountry",
                ZipCode = "OtherZipCode",
                BtcWalletAddress = "OtherBtcWalletAddress",
                Role = "OtherAdmin"
            };

            _context.UserAccountsData.AddRange(existingUserAccountData, otherUser);
            await _context.SaveChangesAsync();

            var myMultiAccount = new UserMultiAccount
            {
                UserAccountDataId = existingUserAccountData.Id,
                MultiAccountName = "myMultiAccountName",
                RefLink = "myReflink12345",
                IsMainAccount = true
            };
            var otherMultiAccount = new UserMultiAccount
            {
                UserAccountDataId = otherUser.Id,
                MultiAccountName = "otherMultiAccountName",
                RefLink = "otherUserReflink12345",
                IsMainAccount = true
            };

            _context.UserMultiAccounts.AddRange(myMultiAccount, otherMultiAccount);
            await _context.SaveChangesAsync();

            var myMatrixPosition = new MatrixPosition
            {
                Id = Guid.NewGuid(),
                UserMultiAccountId = myMultiAccount.Id,
                ParentId = null,
                MatrixLevel = 0,
                DepthLevel = 0,
                Left = 1,
                Right = 6
            };
            _context.MatrixPositions.Add(myMatrixPosition);
            await _context.SaveChangesAsync();
            
            var otherMatrixPosition = new MatrixPosition()
            {
                Id = Guid.NewGuid(),
                UserMultiAccountId = otherMultiAccount.Id,
                ParentId = myMatrixPosition.Id,
                MatrixLevel = 0,
                DepthLevel = 2, // Level 2 (Line B of the main Acc so it's ok)
                Left = 2,
                Right = 5
            };
            _context.MatrixPositions.Add(otherMatrixPosition);
            await _context.SaveChangesAsync();

            var otherMatrixPosition2 = new MatrixPosition()
            {
                Id = Guid.NewGuid(),
                UserMultiAccountId = null,
                ParentId = otherMatrixPosition.Id,
                MatrixLevel = 0,
                DepthLevel = 3, // Line C
                Left = 3,
                Right = 4
            };
            _context.MatrixPositions.Add(otherMatrixPosition2);
            await _context.SaveChangesAsync();
        }
    }
}
