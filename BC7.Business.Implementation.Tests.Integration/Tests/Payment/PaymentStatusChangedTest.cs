using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Payments.Events;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Domain;
using BC7.Security;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
        private PaymentStatusChangedEventHandler _sut;
        private PaymentStatusChangedEvent _event;

        void GivenSystemUnderTest()
        {
            _sut = new PaymentStatusChangedEventHandler(_paymentHistoryRepository, _userAccountDataRepository);
        }

        async Task AndGivenPaymentAndUserAccountInDatabase()
        {
            var payment = new PaymentHistory
            (
                id: Guid.NewGuid(),
                paymentId: Guid.Parse("ab1a5483-fd98-4fe0-b859-240b718c3ac3"),
                orderId: Guid.NewGuid(),
                amountToPay: 100,
                paymentFor: PaymentForHelper.MembershipsFee
            );

            _context.PaymentHistories.Add(payment);

            var user = new UserAccountData(
                id: Guid.Parse("590565b0-df3e-49f0-866c-ececbe696611"),
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
            user.SetPassword("salt", "hash");

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
                PaidAmount = 40,
                AmountToPayInDestinationCurrency = 100,
                AmountToPayInSourceCurrency = 100
            };
        }

        async Task WhenHandlerHandlesTheEvent()
        {
            await _sut.Handle(_event);
        }

        async Task ThenPaymentInDatabaseHasPaidStatusAndPaidAmountValueSetCorrectly()
        {
            var payment = await _context.PaymentHistories.SingleAsync();
            payment.Status.Should().Be(PaymentStatusHelper.Paid);
            payment.PaidAmount.Should().Be(40);
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
