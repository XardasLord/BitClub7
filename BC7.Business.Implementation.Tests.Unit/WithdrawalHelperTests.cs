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

        [Theory]
        [TestCase(0, 0.01)]
        [TestCase(1, 0.03)]
        [TestCase(2, 0.06)]
        [TestCase(3, 0.12)]
        [TestCase(4, 0.25)]
        [TestCase(5, 0.50)]
        [TestCase(6, 1)]
        public void CalculateAmountToWithdraw_WhenCalledWithMatrixLevel_ReturnsCorrectCalculatedValue(int matrixLevel, decimal expectedResult)
        {
            var result = _withdrawalHelper.CalculateAmountToWithdraw(matrixLevel);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [TestCase(0.01, 0.01)]
        [TestCase(0.02, 0.02)]
        [TestCase(0.03, 0.03)]
        [TestCase(0.04, 0.04)]
        [TestCase(0.05, 0.05)]
        [TestCase(0.11, 0.11)]
        public void CalculateAmountToWithdraw_WhenCalledWithAmount_ReturnsCorrectCalculatedValue(decimal amount, decimal expectedResult)
        {
            var result = _withdrawalHelper.CalculateAmountToWithdraw(amount);

            result.Should().Be(expectedResult);
        }
    }
}