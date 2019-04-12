using System;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Users.Commands.CreateMultiAccount;
using BC7.Domain;
using BC7.Security;
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
            _sut = new CreateMultiAccountCommandHandler(_context, _userAccountDataRepository, _userMultiAccountRepository, _matrixPositionRepository, _userMultiAccountHelper, _matrixPositionHelper);
            await CreateUserAndMultiAccountAndMatrixPositionsInDatabase();
            var command = new CreateMultiAccountCommand
            {
                UserAccountId = Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499"),
                RefLink = "otherUserReflink12345"
            };

            var result = await _sut.Handle(command);

            var multiAccount = _context.UserMultiAccounts.Where(x => x.UserAccountDataId == Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499")).ToList();
            result.Should().NotBe(Guid.Empty);
            multiAccount.Count.Should().Be(2);
        }

        private async Task CreateUserAndMultiAccountAndMatrixPositionsInDatabase()
        {
            var existingUserAccountData = new UserAccountData
            (
                id: Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499"),
                login: "ExistingLogin",
                email: "Email",
                firstName: "FirstName",
                lastName: "LastName",
                street: "Street",
                city: "City",
                country: "Country",
                zipCode: "ZipCode",
                btcWalletAddress: "BtcWalletAddress",
                role: UserRolesHelper.User
            );
            existingUserAccountData.SetPassword("salt", "hash");

            var otherUser = new UserAccountData(
                id: Guid.NewGuid(),
                login: "OtherLogin",
                email: "OtherEmail",
                firstName: "OtherFirstName",
                lastName: "OtherLastName",
                street: "OtherStreet",
                city: "OtherCity",
                country: "OtherCountry",
                zipCode: "OtherZipCode",
                btcWalletAddress: "OtherBtcWalletAddress",
                role: UserRolesHelper.User
            );
            otherUser.SetPassword("salt", "hash");

            _context.UserAccountsData.AddRange(existingUserAccountData, otherUser);
            await _context.SaveChangesAsync();

            var myMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: existingUserAccountData.Id,
                sponsorId: null,
                multiAccountName: "myMultiAccountName"
            );
            myMultiAccount.SetReflink("myReflink12345");
            myMultiAccount.SetAsMainAccount();

            var otherMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: otherUser.Id,
                sponsorId: null,
                multiAccountName: "otherMultiAccountName"
            );
            otherMultiAccount.SetReflink("otherUserReflink12345");
            otherMultiAccount.SetAsMainAccount();

            _context.UserMultiAccounts.AddRange(myMultiAccount, otherMultiAccount);
            await _context.SaveChangesAsync();

            // Matrices
            var myMatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: myMultiAccount.Id,
                parentId: null,
                matrixLevel: 0,
                depthLevel: 0,
                left: 1,
                right: 6
            );
            _context.MatrixPositions.Add(myMatrixPosition);
            await _context.SaveChangesAsync();

            var otherMatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherMultiAccount.Id,
                parentId: myMatrixPosition.Id,
                matrixLevel: 0,
                depthLevel: 2, // Level 2 (Line B of the main account so it's ok)
                left: 2,
                right: 5
            );
            _context.MatrixPositions.Add(otherMatrixPosition);
            await _context.SaveChangesAsync();

            var otherMatrixPosition2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: otherMatrixPosition.Id,
                matrixLevel: 0,
                depthLevel: 3, // Line C
                left: 3,
                right: 4
            );
            _context.MatrixPositions.Add(otherMatrixPosition2);
            await _context.SaveChangesAsync();
        }
    }
}
