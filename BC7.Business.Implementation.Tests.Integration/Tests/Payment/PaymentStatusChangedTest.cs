using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Payments.Events;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.Payment
{
    [Story(
        AsA = "As a system",
        IWant = "I want to change the status of the payment in database and amount paid",
        SoThat = "So the payment in database will have changed the status and paidAmount value"
    )]
    public class PaymentStatusChangedTest : BaseIntegration
    {
        private PaymentStatusChangedEventHandler _sut;
        private PaymentStatusChangedEvent _event;

        void GivenSystemUnderTest()
        {
            _sut = new PaymentStatusChangedEventHandler(_paymentHistoryRepository);
        }

        async Task AndGivenPaymentInDatabase()
        {
            var payment = new PaymentHistory
            (
                id: Guid.NewGuid(),
                paymentId: Guid.Parse("ab1a5483-fd98-4fe0-b859-240b718c3ac3"),
                orderId: Guid.NewGuid(),
                amountToPay: 100
            );

            _context.PaymentHistories.Add(payment);
            await _context.SaveChangesAsync();
        }

        void AndGivenEventPrepared()
        {
            _event = new PaymentStatusChangedEvent
            {
                PaymentId = Guid.Parse("ab1a5483-fd98-4fe0-b859-240b718c3ac3"),
                OrderId = Guid.NewGuid(),
                Status = "PAID",
                PaidAmount = 40,
                AmountToPayInDestinationCurrency = 100,
                AmountToPayInSourceCurrency = 100
            };
        }

        async Task WhenHandlerHandlesTheEvent()
        {
            await _sut.Handle(_event);
        }

        async Task AndWhenHandlerHandlesTheEventAgain()
        {
            await _sut.Handle(_event);
        }

        async Task ThenPaymentInDatabaseShouldHasPaidStatusAndPaidAmountValueSetCorrectly()
        {
            var payment = await _context.PaymentHistories.SingleAsync();
            payment.Status.Should().Be("PAID");
            payment.PaidAmount.Should().Be(80);
        }

        [Test]
        public void NotifyPaymentStatusChangedEvent()
        {
            this.BDDfy();
        }
    }
}
