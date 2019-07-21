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
        [TestCase(0, 0.00905)]
        [TestCase(1, 0.02715)]
        [TestCase(2, 0.0543)]
        [TestCase(3, 0.1086)]
        [TestCase(4, 0.22625)]
        [TestCase(5, 0.4525)]
        [TestCase(6, 0.905)]
        public void CalculateAmountToWithdraw_WhenCalledWithMatrixLevel_ReturnsCorrectCalculatedValue(int matrixLevel, decimal expectedResult)
        {
            var result = _withdrawalHelper.CalculateAmountToWithdraw(matrixLevel);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [TestCase(0.01, 0.00905)]
        [TestCase(0.02, 0.0181)]
        [TestCase(0.03, 0.02715)]
        [TestCase(0.04, 0.0362)]
        [TestCase(0.05, 0.04525)]
        [TestCase(0.11, 0.09955)]
        public void CalculateAmountToWithdraw_WhenCalledWithAmount_ReturnsCorrectCalculatedValue(decimal amount, decimal expectedResult)
        {
            var result = _withdrawalHelper.CalculateAmountToWithdraw(amount);

            result.Should().Be(expectedResult);
        }
    }
}