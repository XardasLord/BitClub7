using System;
using BC7.Domain;
using BC7.Security;
using Bogus;

namespace BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator
{
    public class FakerGenerator
    {
        public Faker<UserAccountData> GetUserAccountDataFakerGenerator()
        {
            return new Faker<UserAccountData>("pl")
                .CustomInstantiator(f => new UserAccountData(
                    id: Guid.NewGuid(),
                    firstName: f.Person.FirstName,
                    email: f.Person.Email,
                    login: f.Company.CompanyName(),
                    lastName: f.Person.LastName,
                    street: f.Address.StreetName(),
                    city: f.Address.City(),
                    zipCode: f.Address.ZipCode(),
                    country: f.Address.Country(),
                    btcWalletAddress: f.Finance.BitcoinAddress(),
                    role: UserRolesHelper.Admin));
        }
    }
}
