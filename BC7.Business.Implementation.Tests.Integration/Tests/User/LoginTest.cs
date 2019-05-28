using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Authentications.Commands.Login;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Security.PasswordUtilities;
using FluentAssertions;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.User
{
    public class LoginTest : BaseIntegration
    {
        private const string TestLogin = "Test123";
        private const string TestPassword = "Password12345";

        private LoginCommandHandler _sut;
        private LoginCommand _command;
        private string _result;

        void GivenSystemUnderTest()
        {
            _sut = new LoginCommandHandler(_context, _jwtSettings);
        }

        async Task AndGivenCreatedUserAccountInDatabase()
        {
            var fakerGenerator = new FakerGenerator();
            var hashSalt = PasswordEncryptionUtilities.GenerateSaltedHash(TestPassword);

            var myUserAccountData = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Id, f => Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499"))
                .RuleFor(x => x.Login, f => TestLogin)
                .RuleFor(x => x.Hash, hashSalt.Hash)
                .RuleFor(x => x.Salt, hashSalt.Salt)
                .Generate();

            _context.UserAccountsData.Add(myUserAccountData);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _command = new LoginCommand
            {
                LoginOrEmail = TestLogin,
                Password = TestPassword
            };
        }

        async Task WhenHandlerHandlesTheCommand()
        {
            _result = await _sut.Handle(_command);
        }

        void ThenResultShouldGenerateJwtToken()
        {
            _result.Should().NotBeEmpty();
        }

        [Test]
        public void LoginToAccount()
        {
            this.BDDfy();
        }
    }
}
