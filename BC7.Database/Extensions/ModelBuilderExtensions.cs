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
                    IsMembershipFeePaid = true,
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
                    IsMembershipFeePaid = true,
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
                    IsMembershipFeePaid = true,
                    Role = UserRolesHelper.Root
                }
            );

            var root1MultiAccountId = Guid.NewGuid();
            var root2MultiAccountId = Guid.NewGuid();
            var root3MultiAccountId = Guid.NewGuid();

            modelBuilder.Entity<UserMultiAccount>().HasData(
                new UserMultiAccount
                {
                    Id = root1MultiAccountId,
                    UserAccountDataId = root1Id,
                    MultiAccountName = "LoginRoot1",
                    RefLink = "111111",
                    IsMainAccount = true
                },
                new UserMultiAccount
                {
                    Id = root2MultiAccountId,
                    UserAccountDataId = root2Id,
                    MultiAccountName = "LoginRoot2",
                    RefLink = "222222",
                    IsMainAccount = true
                },
                new UserMultiAccount
                {
                    Id = root3MultiAccountId,
                    UserAccountDataId = root3Id,
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
