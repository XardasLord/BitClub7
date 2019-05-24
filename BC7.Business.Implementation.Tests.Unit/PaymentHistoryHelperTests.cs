using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Helpers;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Domain;
using BC7.Repository;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BC7.Business.Implementation.Tests.Unit
{
    public class PaymentHistoryHelperTests
    {
        private readonly FakerGenerator _fakerGenerator;
        private readonly Mock<IPaymentHistoryRepository> _paymentHistoryRepositoryMock;

        public PaymentHistoryHelperTests()
        {
            _fakerGenerator = new FakerGenerator();
            _paymentHistoryRepositoryMock = new Mock<IPaymentHistoryRepository>(MockBehavior.Strict);
        }

        public Task<bool> Act(int matrixLevelUpgrade, Guid userMultiAccountId)
        {
            var sut = new PaymentHistoryHelper(_paymentHistoryRepositoryMock.Object);

            return sut.DoesUserPaidForMatrixLevelAsync(matrixLevelUpgrade, userMultiAccountId);
        }

        [Test]
        [TestCase("NOT PAID", false)]
        [TestCase("PAID", true)]
        [TestCase("COMPLETED", true)]
        public async Task DoesUserPaidForMatrixLevelAsync_WhenCalledWithGivenStatus_ReturnsCorrectValue(string status, bool expectedValue)
        {
            _paymentHistoryRepositoryMock.Setup(x => x.GetPaymentsByUser(It.IsAny<Guid>()))
                .Returns(
                    Task.FromResult(_fakerGenerator.GetPaymentHistoryFakerGenerator()
                        .RuleFor(x => x.PaymentFor, PaymentForHelper.MatrixLevelPositionsDictionary[0])
                        .RuleFor(x => x.Status, status)
                        .Generate(2)));
            
            var result = await Act(0, Guid.NewGuid());

            result.Should().Be(expectedValue);
        }
    }
}
