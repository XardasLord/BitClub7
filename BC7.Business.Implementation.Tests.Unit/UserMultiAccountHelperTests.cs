using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Helpers;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Database;
using BC7.Repository;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BC7.Business.Implementation.Tests.Unit
{
    public class UserMultiAccountHelperTests
    {
        private readonly FakerGenerator _fakeGenerator;
        private readonly Mock<IBitClub7Context> _contextMock;
        private readonly Mock<IUserAccountDataRepository> _userAccountDataRepositoryMock;

        public UserMultiAccountHelperTests()
        {
            _fakeGenerator = new FakerGenerator();
            _contextMock = new Mock<IBitClub7Context>(MockBehavior.Strict);
            _userAccountDataRepositoryMock = new Mock<IUserAccountDataRepository>(MockBehavior.Strict);
        }

        [Test]
        [TestCase(0, "TestUser")]
        [TestCase(1, "TestUser-001")]
        [TestCase(5, "TestUser-005")]
        public async Task GenerateNextMultiAccountName_WhenCalled_ReturnsCorrectMultiAccountName(int numberOfMultiAccounts, string expectedResult)
        {
            var sut = new UserMultiAccountHelper(_contextMock.Object, _userAccountDataRepositoryMock.Object);
            var userAccountDataId = Guid.NewGuid();
            var multiAccounts = _fakeGenerator.GetUserMultiAccountFakerGenerator().Generate(numberOfMultiAccounts);

            _userAccountDataRepositoryMock.Setup(x => x.GetAsync(userAccountDataId))
                .Returns(
                    Task.FromResult(_fakeGenerator.GetUserAccountDataFakerGenerator()
                        .RuleFor(x => x.Login, "TestUser")
                        .RuleFor(x => x.UserMultiAccounts, multiAccounts)
                        .Generate()));

            var result = await sut.GenerateNextMultiAccountName(userAccountDataId);

            result.Should().Be(expectedResult);
        }
    }
}
