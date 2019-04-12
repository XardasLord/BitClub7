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
        IWant = "I want to buy a position in matrix which is already full",
        SoThat = "So I will have position in newly founded matrix in new sponsor"
    )]
    public class BuyPositionInMatrixWhereIsNoEmptySpaceTest : BaseIntegration
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
                sponsorId: null,
                multiAccountName: "otherMultiAccountName"
            );
            otherMultiAccount.SetReflink("otherUserReflink12345");
            otherMultiAccount.SetAsMainAccount();

            var otherMultiAccount2 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: otherUser.Id,
                sponsorId: null,
                multiAccountName: "otherMultiAccountName2"
            );
            otherMultiAccount2.SetReflink("otherUserReflink123456789");

            var otherMultiAccount3 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: otherUser.Id,
                sponsorId: null,
                multiAccountName: "otherMultiAccountName3"
            );
            otherMultiAccount3.SetReflink("3");

            var myMultiAccount = new UserMultiAccount
            (
                id: Guid.Parse("032d748c-9cef-4a5a-92bd-3fd9a4a0e499"),
                userAccountDataId: existingUserAccountData.Id,
                sponsorId: otherMultiAccount.Id,
                multiAccountName: "myMultiAccountName"
            );
            myMultiAccount.SetAsMainAccount();

            _context.UserMultiAccounts.AddRange(myMultiAccount, otherMultiAccount, otherMultiAccount2, otherMultiAccount3);
            await _context.SaveChangesAsync();

            // Matrices
            var topMatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherMultiAccount.Id,
                parentId: null,
                matrixLevel: 0,
                depthLevel: 0,
                left: 1,
                right: 18
            );
            _context.MatrixPositions.Add(topMatrixPosition);
            await _context.SaveChangesAsync();

            var positionLineA1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherMultiAccount2.Id,
                parentId: topMatrixPosition.Id,
                matrixLevel: 0,
                depthLevel: 1,
                left: 2,
                right: 11
            );
            _context.MatrixPositions.Add(positionLineA1);
            await _context.SaveChangesAsync();

            var positionLineA2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherMultiAccount2.Id,
                parentId: topMatrixPosition.Id,
                matrixLevel: 0,
                depthLevel: 1,
                left: 12,
                right: 17
            );
            _context.MatrixPositions.Add(positionLineA2);
            await _context.SaveChangesAsync();

            var positionLineB1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherMultiAccount3.Id,
                parentId: positionLineA1.Id,
                matrixLevel: 0,
                depthLevel: 2,
                left: 3,
                right: 4
            );
            _context.MatrixPositions.Add(positionLineB1);
            await _context.SaveChangesAsync();

            var positionLineB2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherMultiAccount3.Id,
                parentId: positionLineA1.Id,
                matrixLevel: 0,
                depthLevel: 2,
                left: 5,
                right: 8
            );
            _context.MatrixPositions.Add(positionLineB2);
            await _context.SaveChangesAsync();

            var positionLineC1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB2.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 6,
                right: 7
            );
            _context.MatrixPositions.Add(positionLineC1);
            await _context.SaveChangesAsync();

            var positionLineC2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB2.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 8,
                right: 9
            );
            _context.MatrixPositions.Add(positionLineC2);
            await _context.SaveChangesAsync();

            var positionLineB3 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherMultiAccount3.Id,
                parentId: positionLineA2.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 13,
                right: 14
            );
            _context.MatrixPositions.Add(positionLineB3);
            await _context.SaveChangesAsync();

            var positionLineB4 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherMultiAccount3.Id,
                parentId: positionLineA2.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 15,
                right: 16
            );
            _context.MatrixPositions.Add(positionLineB4);
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

        async Task AndUserHasNewSponsor()
        {
            var userMultiAccount = await _context.UserMultiAccounts.SingleAsync(x => x.Id == _command.UserMultiAccountId);
            userMultiAccount.SponsorId.Should().NotBe(Guid.Parse("d4887060-fb76-429b-95db-113fef65d68d"));
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
                    .And(x => x.AndUserHasNewSponsor())
                .BDDfy();
        }
    }
}
