using System;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Implementation.MatrixPositions.Commands.UpgradeMatrix;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Domain;
using BC7.Security;
using FluentAssertions;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.UpgradingMatrix
{
    [Story(
        AsA = "As a normal user",
        IWant = "I want to upgrade my matrix position to the higher level and my sponsor DID NOT the upgrade before me",
        SoThat = "So I will have position in a higher matrix level in the appropriate structure position based on a very top admin position (left/right)"
    )]
    public class UpgradeMatrixAsNormalUserWhenSponsorDidNotUpgradeBeforeTest : BaseIntegration
    {
        private UpgradeMatrixCommandHandler _sut;
        private UpgradeMatrixCommand _command;
        private UpgradeMatrixResult _result;

        private readonly Guid _userMultiAccountId = Guid.Parse("032d748c-9cef-4a5a-92bd-3fd9a4a0e499");

        void GivenSystemUnderTest()
        {
            var backgroundJobClient = new Mock<IBackgroundJobClient>();

            _sut = new UpgradeMatrixCommandHandler(_userMultiAccountRepository, _matrixPositionRepository, _userAccountDataRepository, _paymentHistoryHelper, _matrixPositionHelper, backgroundJobClient.Object);
        }

        async Task AndGivenCreatedDefaultAccountsAndMatricesInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var adminUserAccountData1 = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Role, UserRolesHelper.Admin)
                .RuleFor(x => x.IsMembershipFeePaid, true)
                .Generate();

            var adminUserAccountData2 = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Role, UserRolesHelper.Admin)
                .RuleFor(x => x.IsMembershipFeePaid, true)
                .Generate();

            var adminUserAccountData3 = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Role, UserRolesHelper.Admin)
                .RuleFor(x => x.IsMembershipFeePaid, true)
                .Generate();

            var adminUserAccountData4 = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Role, UserRolesHelper.Admin)
                .RuleFor(x => x.IsMembershipFeePaid, true)
                .Generate();

            var rootUser = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Role, UserRolesHelper.Root)
                .RuleFor(x => x.IsMembershipFeePaid, true)
                .Generate();

            var sponsorUser = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Role, UserRolesHelper.User)
                .RuleFor(x => x.IsMembershipFeePaid, true)
                .Generate();

            var user1 = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Role, UserRolesHelper.User)
                .RuleFor(x => x.IsMembershipFeePaid, true)
                .Generate();

            var testUser = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Role, UserRolesHelper.User)
                .RuleFor(x => x.IsMembershipFeePaid, true)
                .Generate();

            _context.UserAccountsData.AddRange(adminUserAccountData1, adminUserAccountData2, adminUserAccountData3, adminUserAccountData4, rootUser, sponsorUser, testUser, user1);
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
                id: Guid.NewGuid(),
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

            var sponsorMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: sponsorUser.Id,
                sponsorId: rootMultiAccount3.Id,
                multiAccountName: "sponsorMultiAccountName"
            );
            sponsorMultiAccount.SetAsMainAccount();

            var userMultiAccount1 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: user1.Id,
                sponsorId: adminMultiAccount4.Id,
                multiAccountName: "userMultiAccountName1"
            );
            userMultiAccount1.SetAsMainAccount();

            var testUserMultiAccount = new UserMultiAccount
            (
                id: _userMultiAccountId,
                userAccountDataId: testUser.Id,
                sponsorId: sponsorMultiAccount.Id,
                multiAccountName: "testUserMultiAccountName"
            );
            testUserMultiAccount.SetAsMainAccount();

            _context.UserMultiAccounts.AddRange(adminMultiAccount1, adminMultiAccount2, adminMultiAccount3, adminMultiAccount4, rootMultiAccount1, rootMultiAccount2, rootMultiAccount3, sponsorMultiAccount, userMultiAccount1, testUserMultiAccount);
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
                right: 38
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
                right: 37
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
                right: 36
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
                userMultiAccountId: sponsorMultiAccount.Id,
                parentId: positionLineB4.Id,
                matrixLevel: 0,
                depthLevel: 3,
                left: 26,
                right: 35
            );
            _context.MatrixPositions.Add(positionLineC8);

            var positionLineD1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: userMultiAccount1.Id,
                parentId: positionLineC8.Id,
                matrixLevel: 0,
                depthLevel: 4,
                left: 27,
                right: 32
            );
            _context.MatrixPositions.Add(positionLineD1);

            var positionLineD2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineC8.Id,
                matrixLevel: 0,
                depthLevel: 4,
                left: 33,
                right: 34
            );
            _context.MatrixPositions.Add(positionLineD2);

            var positionLineE1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: testUserMultiAccount.Id,
                parentId: positionLineD1.Id,
                matrixLevel: 0,
                depthLevel: 5,
                left: 28,
                right: 29
            );
            _context.MatrixPositions.Add(positionLineE1);

            var positionLineE2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineD1.Id,
                matrixLevel: 0,
                depthLevel: 5,
                left: 30,
                right: 31
            );
            _context.MatrixPositions.Add(positionLineE2);

            // LVL 1
            var topMatrixPositionLvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: rootMultiAccount1.Id,
                parentId: null,
                matrixLevel: 1,
                depthLevel: 0,
                left: 1,
                right: 34
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
                right: 15
            );
            _context.MatrixPositions.Add(positionLineA1Lvl1);

            var positionLineA2Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: rootMultiAccount3.Id,
                parentId: topMatrixPositionLvl1.Id,
                matrixLevel: 1,
                depthLevel: 1,
                left: 16,
                right: 33
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
                userMultiAccountId: adminMultiAccount2.Id,
                parentId: positionLineA1Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 2,
                left: 9,
                right: 14
            );
            _context.MatrixPositions.Add(positionLineB2Lvl1);

            var positionLineB3Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: adminMultiAccount3.Id,
                parentId: positionLineA2Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 2,
                left: 17,
                right: 26
            );
            _context.MatrixPositions.Add(positionLineB3Lvl1);

            var positionLineB4Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: adminMultiAccount4.Id,
                parentId: positionLineA2Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 2,
                left: 27,
                right: 32
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

            var positionLineC3Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB2Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 3,
                left: 10,
                right: 11
            );
            _context.MatrixPositions.Add(positionLineC3Lvl1);

            var positionLineC4Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB2Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 3,
                left: 12,
                right: 13
            );
            _context.MatrixPositions.Add(positionLineC4Lvl1);

            var positionLineC5Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB3Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 3,
                left: 18,
                right: 19
            );
            _context.MatrixPositions.Add(positionLineC5Lvl1);

            var positionLineC6Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: userMultiAccount1.Id,
                parentId: positionLineB3Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 3,
                left: 20,
                right: 25
            );
            _context.MatrixPositions.Add(positionLineC6Lvl1);

            var positionLineC7Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB4Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 3,
                left: 28,
                right: 29
            );
            _context.MatrixPositions.Add(positionLineC7Lvl1);

            var positionLineC8Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineB4Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 3,
                left: 30,
                right: 31
            );
            _context.MatrixPositions.Add(positionLineC8Lvl1);

            var positionLineD1Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineC6Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 4,
                left: 21,
                right: 22
            );
            _context.MatrixPositions.Add(positionLineD1Lvl1);

            var positionLineD2Lvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                userMultiAccountId: null,
                parentId: positionLineC6Lvl1.Id,
                matrixLevel: 1,
                depthLevel: 4,
                left: 23,
                right: 24
            );
            _context.MatrixPositions.Add(positionLineD2Lvl1);
            await _context.SaveChangesAsync();

            // Payments
            var paymentHistory = fakerGenerator.GetPaymentHistoryFakerGenerator()
                .RuleFor(x => x.OrderId, _userMultiAccountId)
                .RuleFor(x => x.AmountToPay, 10M)
                .RuleFor(x => x.PaidAmount, 10M)
                .RuleFor(x => x.Status, PaymentStatusHelper.Completed)
                .RuleFor(x => x.PaymentFor, PaymentForHelper.MatrixLevelPositionsDictionary[1])
                .Generate();

            _context.PaymentHistories.Add(paymentHistory);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _command = new UpgradeMatrixCommand
            {
                UserMultiAccountId = _userMultiAccountId,
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

        async Task AndUserHasMatrixPositionOnUpgradedLevelUnderRightSideOfTopAdmin()
        {
            var userPositionOnUpgradedMatrix = await _context.MatrixPositions.SingleAsync(x => x.Id == _result.UpgradedMatrixPositionId);
            userPositionOnUpgradedMatrix.UserMultiAccountId.Should().Be(_userMultiAccountId);
            userPositionOnUpgradedMatrix.DepthLevel.Should().Be(3);
            userPositionOnUpgradedMatrix.Left.Should().Be(30);
            userPositionOnUpgradedMatrix.Right.Should().Be(31); // WILL CHANGE on update to 35
        }

        // TODO: Do it via job mock result
        //async Task AndThereAreTwoNewEmptyPositionsUnderUserNewPosition()
        //{
        //    var newPositions = await _context.MatrixPositions
        //        .Where(x => x.MatrixLevel == _command.MatrixLevel)
        //        .Where(x => x.DepthLevel > 3)
        //        .ToListAsync();

        //    newPositions.Count.Should().Be(4);
        //    newPositions.All(x => x.UserMultiAccountId == null).Should().BeTrue();
        //}

        [Test]
        public void UpgradeMatrixAsNormalUserWhenSponsorDidUpgradeBefore()
        {
            this.BDDfy();
        }
    }
}
