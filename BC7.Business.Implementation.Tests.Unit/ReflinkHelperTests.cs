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
        public void GenerateReflink_WhenCalled_ReflinkHasLengthOf32()
        {
            var reflink = _reflinkHelper.GenerateReflink();

            reflink.Length.Should().Be(32);
        }

        [Test]
        public void GenerateReflink_WhenCalled_ReturnsDifferentHashEachTime()
        {
            var hashes = new List<string>();
            
            for (var i = 0; i < 10; i++)
            {
                hashes.Add(_reflinkHelper.GenerateReflink());
            }
            
            hashes.Should().OnlyHaveUniqueItems();
        }
    }
}
