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
            // TODO: Hash and salt for roots
            var root1 = new UserAccountData
            (
                Guid.NewGuid(),
                login: "LoginRoot1",
                email: "EmailRoot1",
                firstName: "FirstNameRoot1",
                lastName: "LastNameRoot1",
                street: "StreetRoot1",
                city: "CityRoot1",
                zipCode: "ZipCodeRoot1",
                country: "CountryRoot1",
                btcWalletAddress: "BtcWalletAddressRoot1",
                role: UserRolesHelper.Root
            );
            var root2 = new UserAccountData
            (
                Guid.NewGuid(),
                login: "LoginRoot2",
                email: "EmailRoot2",
                firstName: "FirstNameRoot2",
                lastName: "LastNameRoot2",
                street: "StreetRoot2",
                city: "CityRoot2",
                zipCode: "ZipCodeRoot2",
                country: "CountryRoot2",
                btcWalletAddress: "BtcWalletAddressRoot2",
                role: UserRolesHelper.Root
            );
            var root3 = new UserAccountData
            (
                Guid.NewGuid(),
                login: "LoginRoot3",
                email: "EmailRoot3",
                firstName: "FirstNameRoot3",
                lastName: "LastNameRoot3",
                street: "StreetRoot3",
                city: "CityRoot3",
                zipCode: "ZipCodeRoot3",
                country: "CountryRoot3",
                btcWalletAddress: "BtcWalletAddressRoot3",
                role: UserRolesHelper.Root
            );
            root1.SetPassword("salt1", "hash1");
            root2.SetPassword("salt2", "hash2");
            root3.SetPassword("salt3", "hash3");
            root1.PaidMembershipFee();
            root2.PaidMembershipFee();
            root3.PaidMembershipFee();
            
            modelBuilder.Entity<UserAccountData>().HasData(root1, root2, root3);

            var root1MultiAccountId = Guid.NewGuid();
            var root2MultiAccountId = Guid.NewGuid();
            var root3MultiAccountId = Guid.NewGuid();

            modelBuilder.Entity<UserMultiAccount>().HasData(
                new UserMultiAccount
                {
                    Id = root1MultiAccountId,
                    UserAccountDataId = root1.Id,
                    MultiAccountName = "LoginRoot1",
                    RefLink = "111111",
                    IsMainAccount = true
                },
                new UserMultiAccount
                {
                    Id = root2MultiAccountId,
                    UserAccountDataId = root2.Id,
                    MultiAccountName = "LoginRoot2",
                    RefLink = "222222",
                    IsMainAccount = true
                },
                new UserMultiAccount
                {
                    Id = root3MultiAccountId,
                    UserAccountDataId = root3.Id,
                    MultiAccountName = "LoginRoot3",
                    RefLink = "333333",
                    IsMainAccount = true
                }
            );

            var root1MatrixPositionId = Guid.NewGuid();
            var root2MatrixPositionId = Guid.NewGuid();
            var root3MatrixPositionId = Guid.NewGuid();

            modelBuilder.Entity<MatrixPosition>().HasData(
                new MatrixPosition
                { // root1
                    Id = root1MatrixPositionId,
                    MatrixLevel = 0,
                    ParentId = null,
                    UserMultiAccountId = root1MultiAccountId,
                    DepthLevel = 0,
                    Left = 1,
                    Right = 14
                },
                new MatrixPosition
                { // root2
                    Id = root2MatrixPositionId,
                    MatrixLevel = 0,
                    ParentId = root1MatrixPositionId,
                    UserMultiAccountId = root2MultiAccountId,
                    DepthLevel = 1,
                    Left = 2,
                    Right = 7
                },
                new MatrixPosition
                { // root3
                    Id = root3MatrixPositionId,
                    MatrixLevel = 0,
                    ParentId = root1MatrixPositionId,
                    UserMultiAccountId = root3MultiAccountId,
                    DepthLevel = 1,
                    Left = 8,
                    Right = 13
                },
                new MatrixPosition
                { // free space
                    Id = Guid.NewGuid(),
                    MatrixLevel = 0,
                    ParentId = root2MatrixPositionId,
                    UserMultiAccountId = null,
                    DepthLevel = 2,
                    Left = 3,
                    Right = 4
                },
                new MatrixPosition
                { // free space
                    Id = Guid.NewGuid(),
                    MatrixLevel = 0,
                    ParentId = root2MatrixPositionId,
                    UserMultiAccountId = null,
                    DepthLevel = 2,
                    Left = 5,
                    Right = 6
                },
                new MatrixPosition
                { // free space
                    Id = Guid.NewGuid(),
                    MatrixLevel = 0,
                    ParentId = root3MatrixPositionId,
                    UserMultiAccountId = null,
                    DepthLevel = 2,
                    Left = 9,
                    Right = 10
                },
                new MatrixPosition
                { // free space
                    Id = Guid.NewGuid(),
                    MatrixLevel = 0,
                    ParentId = root3MatrixPositionId,
                    UserMultiAccountId = null,
                    DepthLevel = 2,
                    Left = 11,
                    Right = 12
                }
            );
        }

        public static void Configuration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccountData>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getutcdate()");
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Login).IsUnique();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Login).IsRequired();
                entity.Property(e => e.Salt).IsRequired();
                entity.Property(e => e.Hash).IsRequired();
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.BtcWalletAddress).IsRequired();
                entity.Property(e => e.Street).IsRequired();
                entity.Property(e => e.City).IsRequired();
                entity.Property(e => e.ZipCode).IsRequired();
                entity.Property(e => e.Country).IsRequired();
                entity.Property(e => e.Role).IsRequired();
            });

            modelBuilder.Entity<UserMultiAccount>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<MatrixPosition>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getutcdate()");
            });
        }
    }
}
