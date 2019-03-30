using System;
using BC7.Entity;
using BC7.Security;
using Microsoft.EntityFrameworkCore;

namespace BC7.Database.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var root1Id = Guid.NewGuid();
            var root2Id = Guid.NewGuid();
            var root3Id = Guid.NewGuid();

            modelBuilder.Entity<UserAccountData>().HasData(
                new UserAccountData
                {
                    Id = root1Id,
                    Hash = "", // TODO
                    Salt = "", // TODO
                    Login = "LoginRoot1",
                    Email = "EmailRoot1",
                    FirstName = "FirstNameRoot1",
                    LastName = "LastNameRoot1",
                    Street = "StreetRoot1",
                    City = "CityRoot1",
                    ZipCode = "ZipCodeRoot1",
                    Country = "CountryRoot1",
                    BtcWalletAddress = "BtcWalletAddressRoot1",
                    Role = UserRolesHelper.Root

                },
                new UserAccountData
                {
                    Id = root2Id,
                    Hash = "", // TODO
                    Salt = "", // TODO
                    Login = "LoginRoot2",
                    Email = "EmailRoot2",
                    FirstName = "FirstNameRoot2",
                    LastName = "LastNameRoot2",
                    Street = "StreetRoot2",
                    City = "CityRoot2",
                    ZipCode = "ZipCodeRoot2",
                    Country = "CountryRoot2",
                    BtcWalletAddress = "BtcWalletAddressRoot2",
                    Role = UserRolesHelper.Root
                },
                new UserAccountData
                {
                    Id = root3Id,
                    Hash = "", // TODO
                    Salt = "", // TODO
                    Login = "LoginRoot3",
                    Email = "EmailRoot3",
                    FirstName = "FirstNameRoot3",
                    LastName = "LastNameRoot3",
                    Street = "StreetRoot3",
                    City = "CityRoot3",
                    ZipCode = "ZipCodeRoot3",
                    Country = "CountryRoot3",
                    BtcWalletAddress = "BtcWalletAddressRoot3",
                    Role = UserRolesHelper.Root
                }
            );

            modelBuilder.Entity<UserMultiAccount>().HasData(
                new UserMultiAccount
                {
                    Id = Guid.NewGuid(),
                    UserAccountDataId = root1Id,
                    MultiAccountName = "LoginRoot1",
                    RefLink = "111111"
                },
                new UserMultiAccount
                {
                    Id = Guid.NewGuid(),
                    UserAccountDataId = root2Id,
                    MultiAccountName = "LoginRoot2",
                    RefLink = "222222"
                },
                new UserMultiAccount
                {
                    Id = Guid.NewGuid(),
                    UserAccountDataId = root3Id,
                    MultiAccountName = "LoginRoot3",
                    RefLink = "333333"
                }
            );
        }
    }
}
