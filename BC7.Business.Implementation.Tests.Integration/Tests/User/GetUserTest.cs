using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Business.Implementation.Users.Requests.GetUser;
using BC7.Business.Models;
using FluentAssertions;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.User
{
    [Story(
        AsA = "As a logged user",
        IWant = "I want to get user data by its ID",
        SoThat = "So user data are returned"
    )]
    public class GetUserTest : BaseIntegration
    {
        private GetUserRequestHandler _sut;
        private GetUserRequest _request;

        private readonly Guid _testUserId = Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499");
        private readonly string _testLogin = "Test123";
        private readonly string _testFirstName = "John";
        private UserAccountDataModel _result;

        void GivenSystemUnderTest()
        {
            _sut = new GetUserRequestHandler(_userAccountDataRepository, _withdrawalRepository, _userMultiAccountHelper, _mapper);
        }

        async Task AndGivenCreatedUserAccountInDatabase()
        {
            var fakerGenerator = new FakerGenerator();

            var userAccountData = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Id, _testUserId)
                .RuleFor(x => x.Login, _testLogin)
                .RuleFor(x => x.FirstName, _testFirstName)
                .Generate();

            _context.UserAccountsData.Add(userAccountData);
            await _context.SaveChangesAsync();
        }

        void AndGivenRequestPrepared()
        {
            _request = new GetUserRequest
            {
                UserId = _testUserId
            };
        }

        async Task WhenHandlerHandlesTheCommand()
        {
            _result = await _sut.Handle(_request);
        }

        void ThenResultHasRequestedUserData()
        {
            _result.Should().NotBeNull();
            _result.Id.Should().Be(_testUserId);
            _result.Login.Should().Be(_testLogin);
            _result.FirstName.Should().Be(_testFirstName);
        }

        [Test]
        public void GetUserData()
        {
            this.BDDfy();
        }
    }
}
