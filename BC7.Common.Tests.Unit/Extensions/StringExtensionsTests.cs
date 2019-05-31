using BC7.Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace BC7.Common.Tests.Unit.Extensions
{
    public class StringExtensionsTests
    {
        [Test]
        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("a", false)]
        [TestCase("example", false)]
        public void IsNullOrWhiteSpace_WhenCalled_ReturnCorrectValue(string inputString, bool expectedValue)
        {
            var result = inputString.IsNullOrWhiteSpace();

            result.Should().Be(expectedValue);
        }
    }
}
