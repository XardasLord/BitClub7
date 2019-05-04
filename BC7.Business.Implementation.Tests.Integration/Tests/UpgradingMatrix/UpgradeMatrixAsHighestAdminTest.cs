using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Domain;
using BC7.Security;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.UpgradingMatrix
{
    [Story(
        AsA = "As the highest admin in a structure (DepthLevel = 2)",
        IWant = "I want to upgrade my matrix position to the higher level",
        SoThat = "So I will have position in a higher matrix level in the same structure position as before which is DepthLevel = 2"
    )]
    public class UpgradeMatrixAsHighestAdminTest : BaseIntegration
    {
        private UpgradeMatrixCommandHandler _sut;
        private UpgradeMatrixCommand _command;
        private UpgradeMatrixResult _result;

        private readonly Guid _adminMultiAccountId = Guid.Parse("032d748c-9cef-4a5a-92bd-3fd9a4a0e499");

        void GivenSystemUnderTest()
        {
            _sut = new UpgradeMatrixCommandHandler(_userMultiAccountRepository, _matrixPositionRepository, _paymentHistoryHelper, _matrixPositionHelper);
        }
        
        async Task AndGivenCreatedDefaultAccountsAndMatricesInDatabase()
        {
            var adminUserAccountData1 = new UserAccountData
            (
                id: Guid.NewGuid(),
                login: "ExistingLogin1",
                email: "Email1",
                firstName: "FirstName",
                lastName: "LastName",
                street: "Street",
                city: "City",
                country: "Country",
                zipCode: "ZipCode",
                btcWalletAddress: "BtcWalletAddress",
                role: UserRolesHelper.Admin
            );
            adminUserAccountData1.SetPassword("salt", "hash");
            adminUserAccountData1.PaidMembershipFee();

            var adminUserAccountData2 = new UserAccountData
            (
                id: Guid.NewGuid(),
                login: "ExistingLogin2",
                email: "Email2",
                firstName: "FirstName",
                lastName: "LastName",
                street: "Street",
                city: "City",
                country: "Country",
                zipCode: "ZipCode",
                btcWalletAddress: "BtcWalletAddress",
                role: UserRolesHelper.Admin
            );
            adminUserAccountData2.SetPassword("salt", "hash");
            adminUserAccountData2.PaidMembershipFee();

            var adminUserAccountData3 = new UserAccountData
            (
                id: Guid.NewGuid(),
                login: "ExistingLogin3",
                email: "Email3",
                firstName: "FirstName",
                lastName: "LastName",
                street: "Street",
                city: "City",
                country: "Country",
                zipCode: "ZipCode",
                btcWalletAddress: "BtcWalletAddress",
                role: UserRolesHelper.Admin
            );
            adminUserAccountData3.SetPassword("salt", "hash");
            adminUserAccountData3.PaidMembershipFee();

            var adminUserAccountData4 = new UserAccountData
            (
                id: Guid.NewGuid(),
                login: "ExistingLogin4",
                email: "Email4",
                firstName: "FirstName",
                lastName: "LastName",
                street: "Street",
                city: "City",
                country: "Country",
                zipCode: "ZipCode",
                btcWalletAddress: "BtcWalletAddress",
                role: UserRolesHelper.Admin
            );
            adminUserAccountData4.SetPassword("salt", "hash");
            adminUserAccountData4.PaidMembershipFee();

            var rootUser = new UserAccountData(
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
                role: UserRolesHelper.Root
            );
            rootUser.SetPassword("salt", "hash");
            rootUser.PaidMembershipFee();

            _context.UserAccountsData.AddRange(adminUserAccountData1, adminUserAccountData2, adminUserAccountData3, adminUserAccountData4, rootUser);
            await _context.SaveChangesAsync();

            // Multi accounts
            var rootMultiAccount1 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: rootUser.Id,
                sponsorId: null,
                multiAccountName: "otherMultiAccountName"
            );
            rootMultiAccount1.SetReflink("otherUserReflink12345");
            rootMultiAccount1.SetAsMainAccount();

            var rootMultiAccount2 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: rootUser.Id,
                sponsorId: null,
                multiAccountName: "otherMultiAccountName2"
            );
            rootMultiAccount2.SetReflink("otherUserReflink123456789");

            var rootMultiAccount3 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: rootUser.Id,
                sponsorId: null,
                multiAccountName: "otherMultiAccountName3"
            );
            rootMultiAccount3.SetReflink("3");

            var adminMultiAccount1 = new UserMultiAccount
            (
                id: _adminMultiAccountId,
                userAccountDataId: adminUserAccountData1.Id,
                sponsorId: rootMultiAccount2.Id,
                multiAccountName: "myMultiAccountName"
            );
            adminMultiAccount1.SetAsMainAccount();

            var adminMultiAccount2 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: adminUserAccountData2.Id,
                sponsorId: rootMultiAccount2.Id,
                multiAccountName: "myMultiAccountName"
            );
            adminMultiAccount2.SetAsMainAccount();

            var adminMultiAccount3 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: adminUserAccountData3.Id,
                sponsorId: rootMultiAccount3.Id,
                multiAccountName: "myMultiAccountName"
            );
            adminMultiAccount3.SetAsMainAccount();

            var adminMultiAccount4 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: adminUserAccountData4.Id,
                sponsorId: rootMultiAccount3.Id,
                multiAccountName: "myMultiAccountName"
            );
            adminMultiAccount4.SetAsMainAccount();

            _context.UserMultiAccounts.AddRange(adminMultiAccount1, adminMultiAccount2, adminMultiAccount3, adminMultiAccount4, rootMultiAccount1, rootMultiAccount2, rootMultiAccount3);
            await _context.SaveChangesAsync();

            // Matrices
            // LVL 0
            var topMatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: rootMultiAccount1.Id,
                parentId: null,
                matrixLevel: 0,
                depthLevel: 0,
                left: 1,
                right: 30
            );
            _context.MatrixPositions.Add(topMatrixPosition);

            var positionLineA1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: rootMultiAccount2.Id,
                parentId: topMatrixPosition.Id,
                matrixLevel: 0,
                depthLevel: 1,
                left: 2,
                right: 15
            );
            _context.MatrixPositions.Add(positionLineA1);

            var positionLineA2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: rootMultiAccount3.Id,
                parentId: topMatrixPosition.Id,
                matrixLevel: 0,
                depthLevel: 1,
                left: 16,
                right: 29
            );
            _context.MatrixPositions.Add(positionLineA2);

            var positionLineB1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: adminMultiAccount1.Id, // TESTED
                parentId: positionLineA1.Id,
                matrixLevel: 0,
                depthLevel: 2,
                left: 3,
                right: 8
            );
            _context.MatrixPositions.Add(positionLineB1);

            var positionLineB2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: adminMultiAccount2.Id,
                parentId: positionLineA1.Id,
                matrixLevel: 0,
                depthLevel: 2,
                left: 9,
                right: 14
            );
            _context.MatrixPositions.Add(positionLineB2);

            var positionLineB3 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: adminMultiAccount3.Id,
                parentId: positionLineA2.Id,
                matrixLevel: 0,
                depthLevel: 2,
                left: 17,
                right: 22
            );
            _context.MatrixPositions.Add(positionLineB3);

            var positionLineB4 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: adminMultiAccount4.Id,
                parentId: positionLineA2.Id,
                matrixLevel: 0,
                depthLevel: 2,
                left: 23,
                right: 28
            );
            _context.MatrixPositions.Add(positionLineB4);

            var positionLineC1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB1.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 4,
                right: 5
            );
            _context.MatrixPositions.Add(positionLineC1);

            var positionLineC2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB1.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 6,
                right: 7
            );
            _context.MatrixPositions.Add(positionLineC2);

            var positionLineC3 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB2.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 10,
                right: 11
            );
            _context.MatrixPositions.Add(positionLineC3);

            var positionLineC4 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB2.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 12,
                right: 13
            );
            _context.MatrixPositions.Add(positionLineC4);

            var positionLineC5 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB3.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 18,
                right: 19
            );
            _context.MatrixPositions.Add(positionLineC5);

            var positionLineC6 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB3.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 20,
                right: 21
            );
            _context.MatrixPositions.Add(positionLineC6);

            var positionLineC7 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB4.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 24,
                right: 25
            );
            _context.MatrixPositions.Add(positionLineC7);

            var positionLineC8 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB4.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 26,
                right: 27
            );
            _context.MatrixPositions.Add(positionLineC8);

            // LVL 1
            var topMatrixPositionLvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: rootMultiAccount1.Id,
                parentId: null,
                matrixLevel: 1,
                depthLevel: 0,
                left: 1,
                right: 18
            );
            _context.MatrixPositions.Add(topMatrixPositionLvl1);

            var positionLineA1Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: rootMultiAccount2.Id,
                parentId: topMatrixPositionLvl1.Id,
                matrixLevel: 1,
                depthLevel: 1,
                left: 2,
                right: 11
            );
            _context.MatrixPositions.Add(positionLineA1Lvl1);

            var positionLineA2Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: rootMultiAccount3.Id,
                parentId: topMatrixPositionLvl1.Id,
                matrixLevel: 1,
                depthLevel: 1,
                left: 12,
                right: 17
            );
            _context.MatrixPositions.Add(positionLineA2Lvl1);

            var positionLineB1Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: adminMultiAccount2.Id,
                parentId: positionLineA1Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 2,
                left: 3,
                right: 8
            );
            _context.MatrixPositions.Add(positionLineB1Lvl1);

            var positionLineB2Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineA1Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 2,
                left: 9,
                right: 10
            );
            _context.MatrixPositions.Add(positionLineB2Lvl1);

            var positionLineB3Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineA2Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 2,
                left: 13,
                right: 14
            );
            _context.MatrixPositions.Add(positionLineB3Lvl1);

            var positionLineB4Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: adminMultiAccount4.Id,
                parentId: positionLineA2Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 2,
                left: 15,
                right: 16
            );
            _context.MatrixPositions.Add(positionLineB4Lvl1);

            var positionLineC1Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB1Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 3,
                left: 4,
                right: 5
            );
            _context.MatrixPositions.Add(positionLineC1Lvl1);

            var positionLineC2Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB1Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 3,
                left: 6,
                right: 7
            );
            _context.MatrixPositions.Add(positionLineC2Lvl1);
            await _context.SaveChangesAsync();

            // Payments
            var paymentHistory = new PaymentHistory
            (
                id: Guid.NewGuid(),
                paymentId: Guid.NewGuid(),
                orderId: _adminMultiAccountId,
                amountToPay: 10,
                paymentFor: PaymentForHelper.MatrixLevelPositionsDictionary[1]
            );
            paymentHistory.Paid(10);
            paymentHistory.ChangeStatus(PaymentStatusHelper.Completed);

            _context.PaymentHistories.Add(paymentHistory);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _command = new UpgradeMatrixCommand
            {
                UserMultiAccountId = _adminMultiAccountId,
                MatrixLevel = 1
            };
        }

        async Task WhenHandlerHandlesTheCommand()
        {
            _result = await _sut.Handle(_command);
        }

        void ThenResultHasGuidWithMatrixPositionUpgradedOnTheHigherLevel()
        {
            _result.UpgradedMatrixPositionId.Should().NotBeEmpty();
        }

        void AndResultHasNoErrorMessage()
        {
            _result.ErrorMsg.Should().BeNullOrEmpty();
        }

        async Task AndAdminHasMatrixPositionOnUpgradedLevel()
        {
            var adminPositionOnUpgradedMatrix = await _context.MatrixPositions.SingleOrDefaultAsync(x => x.Id == _result.UpgradedMatrixPositionId);
            adminPositionOnUpgradedMatrix.UserMultiAccountId.Should().Be(_adminMultiAccountId);
            adminPositionOnUpgradedMatrix.DepthLevel.Should().Be(2);
        }

        [Test]
        public void UpgradeMatrixAsHighestAdmin()
        {
            this.BDDfy();
        }
    }
}
