using System;
using System.Threading.Tasks;
using BC7.Business.Implementation.Authentications.Commands.Login;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator;
using BC7.Security.PasswordUtilities;
using FluentAssertions;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.Login
{
    public class LoginTest : BaseIntegration
    {
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

            var myUserAccountData = fakerGenerator.GetUserAccountDataFakerGenerator()
                .RuleFor(x => x.Id, f => Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499"))
                .RuleFor(x => x.Login, f => "Test123")
                .Generate();

            var hashSalt = PasswordEncryptionUtilities.GenerateSaltedHash("Password12345");
            myUserAccountData.SetPassword(hashSalt.Salt, hashSalt.Hash);

            _context.UserAccountsData.Add(myUserAccountData);
            await _context.SaveChangesAsync();
        }

        void AndGivenCommandPrepared()
        {
            _command = new LoginCommand
            {
                LoginOrEmail = "Test123",
                Password = "Password12345"
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
