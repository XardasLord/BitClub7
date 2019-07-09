using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.MatrixPositions.Commands.BuyPositionInMatrix;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Domain;
using FluentAssertions;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Moq;
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
            var backgroundJobClient = new Mock<IBackgroundJobClient>();

            _sut = new BuyPositionInMatrixCommandHandler(
                _userMultiAccountRepository, 
                _userAccountDataRepository, 
                _matrixPositionRepository, 
                _matrixPositionHelper, 
                _paymentHistoryHelper, 
                backgroundJobClient.Object);
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
                .RuleFor(x => x.IsMainAccount, true)
                .Generate();

            var myMultiAccount = fakerGenerator.GetUserMultiAccountFakerGenerator()
                .RuleFor(x => x.Id, Guid.Parse("032d748c-9cef-4a5a-92bd-3fd9a4a0e499"))
                .RuleFor(x => x.UserAccountDataId, existingUserAccountData.Id)
                .RuleFor(x => x.SponsorId, otherMultiAccount.Id)
                .RuleFor(x => x.RefLink, null as string)
                .RuleFor(x => x.IsMainAccount, true)
                .Generate();

            _context.UserMultiAccounts.AddRange(myMultiAccount, otherMultiAccount, otherMultiAccount2);
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
            userMultiAccount.SponsorId.Should().Be("d4887060-fb76-429b-95db-113fef65d68d");
        }

        // TODO: Do it via job mock result
        //async Task AndThereShouldBeTwoNewPositionsInMatrix()
        //{
        //    var positionsCount = await _context.MatrixPositions.CountAsync();
        //    positionsCount.Should().Be(5);
        //}

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
                    //.And(x => x.AndThereShouldBeTwoNewPositionsInMatrix())
                .BDDfy();
        }
    }
}
