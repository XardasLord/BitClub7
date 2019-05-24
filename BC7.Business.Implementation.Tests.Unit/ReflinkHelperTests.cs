using System.Collections.Generic;
using BC7.Business.Implementation.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace BC7.Business.Implementation.Tests.Unit
{
    public class ReflinkHelperTests
    {
        private readonly ReflinkHelper _reflinkHelper;

        public ReflinkHelperTests()
        {
            _reflinkHelper = new ReflinkHelper();
        }

        [Test]
        public void GenerateReflink_WhenCalled_ReturnsDifferentHashEachTime()
        {
            // Arrange
            var hashes = new List<string>();

            // Act
            for (var i = 0; i < 10; i++)
            {
                hashes.Add(_reflinkHelper.GenerateReflink());
            }

            // Assert
            hashes.Should().OnlyHaveUniqueItems();
        }
    }
}
