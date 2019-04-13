using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC7.Business.Implementation.Authentications.Commands.Login;
using BC7.Business.Implementation.Tests.Integration.Base;
using BC7.Business.Implementation.Users.Requests.GetMultiAccounts;
using BC7.Business.Models;
using BC7.Domain;
using BC7.Security;
using BC7.Security.PasswordUtilities;
using FluentAssertions;
using NUnit.Framework;
using TestStack.BDDfy;

namespace BC7.Business.Implementation.Tests.Integration.Tests.GettingMultiAccountsForUser
{
    public class GetMultiAccountsForUserTest : BaseIntegration
    {
        private GetMultiAccountsRequestHandler _sut;
        private GetMultiAccountsRequest _request;
        private IEnumerable<UserMultiAccountModel> _result;

        void GivenSystemUnderTest()
        {
            _sut = new GetMultiAccountsRequestHandler(_mapper, _userAccountDataRepository);
        }
        
        async Task AndGivenCreatedDefaultAccountsInDatabase()
        {
            var hashSalt = PasswordEncryptionUtilities.GenerateSaltedHash("Password12345");
            var myUserAccountData = new UserAccountData
            (
                id: Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499"),
                login: "Test123",
                email: "Email",
                firstName: "FirstName",
                lastName: "LastName",
                street: "Street",
                city: "City",
                country: "Country",
                zipCode: "ZipCode",
                btcWalletAddress: "BtcWalletAddress",
                role: UserRolesHelper.User
            );
            myUserAccountData.SetPassword(hashSalt.Salt, hashSalt.Hash);

            var otherUser = new UserAccountData(
                id: Guid.NewGuid(),
                login: "OtherLogin",
                email: "OtherEmail",
                firstName: "OtherFirstName",
                lastName: "OtherLastName",
                street: "OtherStreet",
                city: "OtherCity",
                country: "OtherCountry",
                zipCode: "OtherZipCode",
                btcWalletAddress: "OtherBtcWalletAddress",
                role: UserRolesHelper.User
            );
            otherUser.SetPassword("salt", "hash");

            _context.UserAccountsData.AddRange(myUserAccountData, otherUser);
            await _context.SaveChangesAsync();

            // Multi accounts
            var otherMultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: otherUser.Id,
                sponsorId: null,
                multiAccountName: "otherMultiAccountName"
            );
            otherMultiAccount.SetReflink("FIRST_REFLINK");
            otherMultiAccount.SetAsMainAccount();

            var myMultiAccount1 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: myUserAccountData.Id,
                sponsorId: null,
                multiAccountName: "myMultiAccount1"
            );

            var myMultiAccount2 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: myUserAccountData.Id,
                sponsorId: null,
                multiAccountName: "myMultiAccount2"
            );

            var myMultiAccount3 = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: myUserAccountData.Id,
                sponsorId: null,
                multiAccountName: "myMultiAccount3"
            );

            _context.UserMultiAccounts.AddRange(otherMultiAccount, myMultiAccount1, myMultiAccount2, myMultiAccount3);
            await _context.SaveChangesAsync();
        }

        void AndGivenRequestPrepared()
        {
            _request = new GetMultiAccountsRequest
            {
                UserAccountId = Guid.Parse("042d748c-9cef-4a5a-92bd-3fd9a4a0e499")
            };
        }

        async Task WhenHandlerHandlesTheCommand()
        {
            _result = await _sut.Handle(_request);
        }

        void ThenResultShouldReturnThreeMultiAccounts()
        {
            _result.Count().Should().Be(3);
        }

        [Test]
        public void LoginToAccount()
        {
            this.BDDfy();
        }
    }
}
