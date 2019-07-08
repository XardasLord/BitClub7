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

        public string Act()
        {
            return _reflinkHelper.GenerateReflink();
        }

        [Test]
        public void GenerateReflink_WhenCalled_ReflinkHasLengthOf16()
        {
            var reflink = Act();

            reflink.Length.Should().Be(16);
        }

        [Test]
        public void GenerateReflink_WhenCalled_ReturnsDifferentHashEachTime()
        {
            var hashes = new List<string>();
            
            for (var i = 0; i < 20; i++)
            {
                hashes.Add(Act());
            }
            
            hashes.Should().OnlyHaveUniqueItems();
        }
    }
}
