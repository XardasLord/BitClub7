using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Jobs;
using BC7.Business.Implementation.Payments.Events;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Domain;
using FluentAssertions;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.Payment
{
    [Story(
        AsA = "As a system",
        IWant = "I want to change the status of the payment in database and amount paid for MembershipsFee payment",
        SoThat = "So the payment in database will have changed the status and paidAmount value and also will set MembershipFee flag for user to yes"
    )]
    public class PaymentStatusChangedTest : BaseIntegration
    {
        private PaymentStatusChangedEventJob _sut;
        private PaymentStatusChangedEvent _event;

        void GivenSystemUnderTest()
        {
            var backgroundJobClient = new Mock<IBackgroundJobClient>();

            _sut = new PaymentStatusChangedEventJob(_paymentHistoryRepository, _userAccountDataRepository, backgroundJobClient.Object);
        }

        async Task AndGivenPaymentAndUserAccountInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var payment = fakerGenerator.GetPaymentHistoryFakerGenerator()
                .RuleFor(x => x.PaymentId, f => Guid.Parse("ab1a5483-fd98-4fe0-b859-240b718c3ac3"))
                .RuleFor(x => x.AmountToPay, f => 0.008M)
                .RuleFor(x => x.PaymentFor, f => PaymentForHelper.MembershipsFee)
                .Generate();

            _context.PaymentHistories.Add(payment);

            var user = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Id, f => Guid.Parse("590565b0-df3e-49f0-866c-ececbe696611"))
                .Generate();

            _context.UserAccountsData.Add(user);
            await _context.SaveChangesAsync();
        }

        void AndGivenEventPrepared()
        {
            _event = new PaymentStatusChangedEvent
            {
                PaymentId = Guid.Parse("ab1a5483-fd98-4fe0-b859-240b718c3ac3"),
                OrderId = Guid.Parse("590565b0-df3e-49f0-866c-ececbe696611"),
                Status = PaymentStatusHelper.Paid,
                PaidAmount = 0.004M,
                AmountToPayInDestinationCurrency = 0.008M,
                AmountToPayInSourceCurrency = 0.008M
            };
        }

        async Task WhenHandlerHandlesTheEvent()
        {
            await _sut.Execute(_event, null);
        }

        async Task ThenPaymentInDatabaseHasPaidStatusAndPaidAmountValueSetCorrectly()
        {
            var payment = await _context.PaymentHistories.SingleAsync();
            payment.Status.Should().Be(PaymentStatusHelper.Paid);
            payment.PaidAmount.Should().Be(0.004M);
        }

        async Task AndUserHasMembershipsFeeSetToTrue()
        {
            var user = await _context.UserAccountsData.SingleAsync();
            user.IsMembershipFeePaid.Should().BeTrue();
        }

        [Test]
        public void NotifyPaymentStatusChangedEvent()
        {
            this.BDDfy();
        }
    }
}
