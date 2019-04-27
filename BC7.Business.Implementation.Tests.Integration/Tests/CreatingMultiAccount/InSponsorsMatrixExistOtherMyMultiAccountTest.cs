using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Users.Commands.CreateMultiAccount;
using BC7.Domain;
using BC7.Security;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.CreatingMultiAccount
{
    [Story(
        AsA = "As a user",
        IWant = "I want to create new multi account when in sponsor's matrix exist other of my multi account",
        SoThat = "So the account will be created in the other sponsor's matrix"
    )]
    public class InSponsorsMatrixExistOtherMyMultiAccountTest : BaseIntegration
    {
        private CreateMultiAccountCommandHandler _sut;
        private CreateMultiAccountCommand _command;
        private Guid _result;

        void GivenSystemUnderTest()
        {
            _sut = new CreateMultiAccountCommandHandler(_context, _userAccountDataRepository,
                _userMultiAccountRepository, _matrixPositionRepository, _userMultiAccountHelper, _matrixPositionHelper);
        }

        [Given(StepTitle = "And given created default accounts and matrices in database prepared")]
        async Task AndGivenCreatedDefaultAccountsAndMatricesInDatabase()
        {
            var myUserAccountData = new UserAccountData
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
            myUserAccountData.SetPassword("salt", "hash");
            myUserAccountData.PaidMembershipFee();

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
            otherUser.PaidMembershipFee();

            _context.UserAccountsData.AddRange(myUserAccountData, otherUser);
            await _context.SaveChangesAsync();

            // Multi accounts
            var otherMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: otherUser.Id,
                sponsorId: null,
                multiAccountName: "otherMultiAccountName"
            );
            otherMultiAccount.SetReflink("FIRST_REFLINK");
            otherMultiAccount.SetAsMainAccount();

            var otherSecondMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: otherUser.Id,
                sponsorId: null,
                multiAccountName: "otherMultiAccountName1"
            );
            otherSecondMultiAccount.SetReflink("SECOND_REFLINK");
            
            var otherThirdMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: otherUser.Id,
                sponsorId: null,
                multiAccountName: "otherMultiAccountName2"
            );
            otherThirdMultiAccount.SetReflink("THIRD_REFLINK");

            var otherFourthMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: otherUser.Id,
                sponsorId: null,
                multiAccountName: "otherMultiAccountName3"
            );
            otherFourthMultiAccount.SetReflink("FOURTH_REFLINK");

            var myMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: myUserAccountData.Id,
                sponsorId: otherMultiAccount.Id,
                multiAccountName: "myMultiAccountName"
            );
            myMultiAccount.SetReflink("myReflink12345");
            myMultiAccount.SetAsMainAccount();

            _context.UserMultiAccounts.AddRange(otherMultiAccount, otherSecondMultiAccount, otherThirdMultiAccount, otherFourthMultiAccount, myMultiAccount);
            await _context.SaveChangesAsync();

            // Matrices
            var rootMatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherMultiAccount.Id,
                parentId: null,
                matrixLevel: 0,
                depthLevel: 0,
                left: 1,
                right: 20
            );
            _context.MatrixPositions.Add(rootMatrixPosition);
            await _context.SaveChangesAsync();

            var otherMatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: myMultiAccount.Id,
                parentId: rootMatrixPosition.Id,
                matrixLevel: 0,
                depthLevel: 1,
                left: 2,
                right: 11
            );
            _context.MatrixPositions.Add(otherMatrixPosition);
            await _context.SaveChangesAsync();

            var otherMatrixPosition2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherSecondMultiAccount.Id,
                parentId: otherMatrixPosition.Id,
                matrixLevel: 0,
                depthLevel: 2,
                left: 3,
                right: 8
            );
            _context.MatrixPositions.Add(otherMatrixPosition2);
            await _context.SaveChangesAsync();

            var otherMatrixPosition3 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: otherMatrixPosition2.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 4,
                right: 5
            );
            _context.MatrixPositions.Add(otherMatrixPosition3);
            await _context.SaveChangesAsync();

            var otherMatrixPosition4 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: otherMatrixPosition2.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 6,
                right: 7
            );
            _context.MatrixPositions.Add(otherMatrixPosition4);
            await _context.SaveChangesAsync();

            var otherMatrixPosition5 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: otherMatrixPosition.Id,
                matrixLevel: 0,
                depthLevel: 2,
                left: 9,
                right: 10
            );
            _context.MatrixPositions.Add(otherMatrixPosition5);
            await _context.SaveChangesAsync();

            var otherMatrixPosition6 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherThirdMultiAccount.Id,
                parentId: rootMatrixPosition.Id,
                matrixLevel: 0,
                depthLevel: 1,
                left: 12,
                right: 19
            );
            _context.MatrixPositions.Add(otherMatrixPosition6);
            await _context.SaveChangesAsync();

            var otherMatrixPosition7 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: otherMatrixPosition6.Id,
                matrixLevel: 0,
                depthLevel: 2,
                left: 13,
                right: 14
            );
            _context.MatrixPositions.Add(otherMatrixPosition7);
            await _context.SaveChangesAsync();

            var otherMatrixPosition8 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: otherFourthMultiAccount.Id,
                parentId: otherMatrixPosition6.Id,
                matrixLevel: 0,
                depthLevel: 2,
                left: 15,
                right: 18
            );
            _context.MatrixPositions.Add(otherMatrixPosition8);
            await _context.SaveChangesAsync();

            var otherMatrixPosition9 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: otherMatrixPosition8.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 16,
                right: 17
            );
            _context.MatrixPositions.Add(otherMatrixPosition9);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _command = new CreateMultiAccountCommand
            {
                UserAccountId = Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499"),
                SponsorReflink = "FIRST_REFLINK"
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

        async Task AndAccountShouldExistInDatabaseWithThisId()
        {
            var multiAccount = await _context.UserMultiAccounts.SingleOrDefaultAsync(x => x.Id == _result);
            multiAccount.Should().NotBeNull();
        }

        async Task AndAccountShouldHasNewSponsor()
        {
            var multiAccount = await _context.UserMultiAccounts.Include(x => x.Sponsor).SingleAsync(x => x.Id == _result);
            multiAccount.Sponsor.RefLink.Should().NotBe(_command.SponsorReflink);
            multiAccount.Sponsor.RefLink.Should().Be("FOURTH_REFLINK");
        }

        [Test]
        public void CreateNewMultiAccount()
        {
            this.BDDfy();
        }
    }
}
