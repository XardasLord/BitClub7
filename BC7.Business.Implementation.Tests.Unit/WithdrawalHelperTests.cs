using BC7.Business.Implementation.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace BC7.Business.Implementation.Tests.Unit
{
    public class WithdrawalHelperTests
    {
        private readonly WithdrawalHelper _withdrawalHelper;

        public WithdrawalHelperTests()
        {
            _withdrawalHelper = new WithdrawalHelper();
        }

        public decimal Act(int matrixLevel)
        {
            return _withdrawalHelper.CalculateAmountToWithdraw(matrixLevel);
        }

        [Theory]
        [TestCase(0, 0.00905)]
        [TestCase(1, 0.02715)]
        [TestCase(2, 0.0543)]
        [TestCase(3, 0.1086)]
        [TestCase(4, 0.22625)]
        [TestCase(5, 0.4525)]
        [TestCase(6, 0.905)]
        public void CalculateAmountToWithdraw_WhenCalled_ReturnsCorrectCalculatedValue(int matrixLevel, decimal expectedResult)
        {
            var result = Act(matrixLevel);

            result.Should().Be(expectedResult);
        }
    }
}