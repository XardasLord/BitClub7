using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Domain;
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
    public class InSponsorsMatrixThereIsNoEmptySpaceTest : BaseIntegration
    {
        private BuyPositionInMatrixCommandHandler _sut;
        private BuyPositionInMatrixCommand _command;
        private Guid _result;

        void GivenSystemUnderTest()
        {
            _sut = new BuyPositionInMatrixCommandHandler(_userMultiAccountRepository, _userAccountDataRepository, _matrixPositionRepository, _matrixPositionHelper, _paymentHistoryHelper, _mediator);
        }

        [Given(StepTitle = "And given created default accounts and matrices in database")]
        async Task AndGivenCreatedDefaultAccountsAndMatricesInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var existingUserAccountData = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.IsMembershipFeePaid, true)
                .Generate();

            var otherUser = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.IsMembershipFeePaid, true)
                .Generate();

            _context.UserAccountsData.AddRange(existingUserAccountData, otherUser);
            await _context.SaveChangesAsync();

            // Multi accounts
            var otherMultiAccount = fakerGenerator.GetUserMultiAccountFakerGenerator()
                .RuleFor(x => x.Id, Guid.Parse("d4887060-fb76-429b-95db-113fef65d68d"))
                .RuleFor(x => x.UserAccountDataId, otherUser.Id)
                .RuleFor(x => x.IsMainAccount, true)
                .Generate();

            var otherMultiAccount2 = fakerGenerator.GetUserMultiAccountFakerGenerator()
                .RuleFor(x => x.UserAccountDataId, otherUser.Id)
                .Generate();

            var otherMultiAccount3 = fakerGenerator.GetUserMultiAccountFakerGenerator()
                .RuleFor(x => x.UserAccountDataId, otherUser.Id)
                .Generate();

            var myMultiAccount = fakerGenerator.GetUserMultiAccountFakerGenerator()
                .RuleFor(x => x.Id, Guid.Parse("032d748c-9cef-4a5a-92bd-3fd9a4a0e499"))
                .RuleFor(x => x.UserAccountDataId, existingUserAccountData.Id)
                .RuleFor(x => x.SponsorId, otherMultiAccount.Id)
                .RuleFor(x => x.RefLink, null as string)
                .RuleFor(x => x.IsMainAccount, true)
                .Generate();

            _context.UserMultiAccounts.AddRange(myMultiAccount, otherMultiAccount, otherMultiAccount2, otherMultiAccount3);
            await _context.SaveChangesAsync();

            // Payments
            var payment = fakerGenerator.GetPaymentHistoryFakerGenerator()
                .RuleFor(x => x.PaymentFor, PaymentForHelper.MatrixLevelPositionsDictionary[0])
                .RuleFor(x => x.Status, PaymentStatusHelper.Paid)
                .RuleFor(x => x.OrderId, Guid.Parse("032d748c-9cef-4a5a-92bd-3fd9a4a0e499"))
                .Generate();

            _context.PaymentHistories.Add(payment);
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

        async Task AndThereShouldBeTwoNewPositionsInMatrix()
        {
            var positionsCount = await _context.MatrixPositions.CountAsync();
            positionsCount.Should().Be(11);
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
                    .And(x => AndThereShouldBeTwoNewPositionsInMatrix())
                .BDDfy();
        }
    }
}
