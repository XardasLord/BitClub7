using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Domain;
using BC7.Security;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.BuyingPositionInMatrix
{
    [Story(
        AsA = "As a multi account owner",
        IWant = "I want to buy a position in matrix",
        SoThat = "So I will have position in matrix"
        )]
    public class BuyPositionInMatrixTest : BaseIntegration
    {
        private BuyPositionInMatrixCommandHandler _sut;
        private BuyPositionInMatrixCommand _command;
        private Guid _result;

        void GivenSystemUnderTest()
        {
            _sut = new BuyPositionInMatrixCommandHandler(_userMultiAccountRepository, _userAccountDataRepository, _matrixPositionRepository, _matrixPositionHelper, _mediator);
        }

        [Given(StepTitle = "And given created default accounts and matrices in database")]
        async Task AndGivenCreatedDefaultAccountsAndMatricesInDatabase()
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

            // Multi accounts
            var otherMultiAccount = new UserMultiAccount
            (
                id: Guid.Parse("d4887060-fb76-429b-95db-113fef65d68d"),
                userAccountDataId: otherUser.Id,
                userMultiAccountInvitingId: null,
                multiAccountName: "otherMultiAccountName"
            );
            otherMultiAccount.SetReflink("otherUserReflink12345");
            otherMultiAccount.SetAsMainAccount();

            var otherMultiAccount2 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: otherUser.Id,
                userMultiAccountInvitingId: null,
                multiAccountName: "otherMultiAccountName2"
            );
            otherMultiAccount2.SetReflink("otherUserReflink123456789");

            var myMultiAccount = new UserMultiAccount
            (
                id: Guid.Parse("032d748c-9cef-4a5a-92bd-3fd9a4a0e499"),
                userAccountDataId: existingUserAccountData.Id,
                userMultiAccountInvitingId: otherMultiAccount.Id,
                multiAccountName: "myMultiAccountName"
            );
            myMultiAccount.SetAsMainAccount();

            _context.UserMultiAccounts.AddRange(myMultiAccount, otherMultiAccount, otherMultiAccount2);
            await _context.SaveChangesAsync();

            // Matrices
            var myMatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherMultiAccount2.Id,
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

        void AndGivenCommandPrepared()
        {
            _command = new BuyPositionInMatrixCommand
            {
                UserMultiAccountId = Guid.Parse("032d748c-9cef-4a5a-92bd-3fd9a4a0e499"),
                MatrixLevel = 0
            };
        }
        
        async Task WhenHandlerHandlesTheCommand()
        {
           _result = await _sut.Handle(_command);
        }

        void ThenResultShouldBeGuidWithMatrixPositionBought()
        {
            _result.Should().NotBeEmpty();
        }

        async Task AndPositionShouldHasAssignedAccountId()
        {
            var matrixPosition = await _context.MatrixPositions.SingleAsync(x => x.Id == _result);
            matrixPosition.UserMultiAccountId.Should().Be(_command.UserMultiAccountId);
        }

        async Task AndUserHasTheSameSponsor()
        {
            var userMultiAccount = await _context.UserMultiAccounts.SingleAsync(x => x.Id == _command.UserMultiAccountId);
            userMultiAccount.UserMultiAccountInvitingId.Should().Be("d4887060-fb76-429b-95db-113fef65d68d");
        }
        
        [Test]
        public void BuyPositionInMatrix()
        {
            this.Given(x => x.GivenSystemUnderTest())
                    .And(x => x.AndGivenCreatedDefaultAccountsAndMatricesInDatabase())
                    .And(x => x.AndGivenCommandPrepared())
                .When(x => x.WhenHandlerHandlesTheCommand())
                .Then(x => x.ThenResultShouldBeGuidWithMatrixPositionBought())
                    .And(x => x.AndPositionShouldHasAssignedAccountId())
                    .And(x => x.AndUserHasTheSameSponsor())
                .BDDfy();
        }
    }
}
