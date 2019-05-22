using System;
using BC7.Domain;
using BC7.Security;
using Bogus;

namespace BC7.Business.Implementation.Tests.Integration.FakerSeedGenerator
{
    public class FakerGenerator
    {
        private const string Local = "pl";

        public Faker<UserAccountData> GetUserAccountDataFakerGenerator()
        {
            return new Faker<UserAccountData>(Local)
                .CustomInstantiator(f => new UserAccountData(
                    id: Guid.NewGuid(),
                    firstName: f.Person.FirstName,
                    email: f.Person.Email,
                    login: f.Internet.UserName(),
                    lastName: f.Person.LastName,
                    street: f.Address.StreetName(),
                    city: f.Address.City(),
                    zipCode: f.Address.ZipCode(),
                    country: f.Address.Country(),
                    btcWalletAddress: f.Finance.BitcoinAddress(),
                    role: UserRolesHelper.User))
                .RuleFor(x => x.Hash, f => f.Random.Hash())
                .RuleFor(x => x.Salt, f => f.Random.Hash())
                .RuleFor(x => x.IsMembershipFeePaid, false);
        }

        public Faker<PaymentHistory> GetPaymentHistoryFakerGenerator()
        {
            return new Faker<PaymentHistory>(Local)
                .CustomInstantiator(f => new PaymentHistory(
                    id: Guid.NewGuid(),
                    paymentId: Guid.NewGuid(),
                    orderId: Guid.NewGuid(),
                    amountToPay: f.Finance.Amount(0.01M, 10M),
                    paymentFor: PaymentForHelper.MembershipsFee));
        }

        public Faker<UserMultiAccount> GetUserMultiAccountFakerGenerator()
        {
            return new Faker<UserMultiAccount>(Local)
                .CustomInstantiator(f => new UserMultiAccount(
                    id: Guid.NewGuid(),
                    userAccountDataId: Guid.NewGuid(),
                    sponsorId: null,
                    multiAccountName: f.Person.UserName))
                .RuleFor(x => x.RefLink, f => f.Random.Hash())
                .RuleFor(x => x.IsMainAccount, false);
        }
    }
}
