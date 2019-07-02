using System.Collections.Generic;
using BC7.Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace BC7.Common.Tests.Unit.Extensions
{
    public class LinqExtensionsTests
    {
        [Test]
        public void ContainsAll_ListAContainsAllElementsFromListB_ReturnTrue()
        {
            var listA = new List<int> { 1, 2, 3, 4, 5 };
            var listB = new List<int> { 2, 3, 4 };

            var result = listA.ContainsAll(listB);

            result.Should().BeTrue();
        }

        [Test]
        public void ContainsAll_ListADoesNotContainAllElementsFromListB_ReturnFalse()
        {
            var listA = new List<int> { 1, 2, 3, 4, 5 };
            var listB = new List<int> { 2, 3, 4, 9 };

            var result = listA.ContainsAll(listB);

            result.Should().BeFalse();
        }
    }
}
