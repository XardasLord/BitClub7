using System;
using BC7.Domain;
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


            var root1MultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: root1.Id,
                sponsorId: null,
                multiAccountName: "LoginRoot1"
            );
            root1MultiAccount.SetAsMainAccount();
            root1MultiAccount.SetReflink("111111");

            var root2MultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: root2.Id,
                sponsorId: null,
                multiAccountName: "LoginRoot2"
            );
            root2MultiAccount.SetAsMainAccount();
            root2MultiAccount.SetReflink("222222");

            var root3MultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: root3.Id,
                sponsorId: null,
                multiAccountName: "LoginRoot3"
            );
            root3MultiAccount.SetAsMainAccount();
            root3MultiAccount.SetReflink("333333");

            modelBuilder.Entity<UserMultiAccount>().HasData(root1MultiAccount, root2MultiAccount, root3MultiAccount);


            var root1MatrixPosition = new MatrixPosition
            (
                // root1
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: null,
                userMultiAccountId: root1MultiAccount.Id,
                depthLevel: 0,
                left: 1,
                right: 14
            );
            var root2MatrixPosition = new MatrixPosition
            (
                // root2
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: root1MatrixPosition.Id,
                userMultiAccountId: root2MultiAccount.Id,
                depthLevel: 1,
                left: 2,
                right: 7
            );
            var root3MatrixPosition = new MatrixPosition
            (
                // root3
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: root1MatrixPosition.Id,
                userMultiAccountId: root3MultiAccount.Id,
                depthLevel: 1,
                left: 8,
                right: 13
            );
            var empty1MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: root2MatrixPosition.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 3,
                right: 4
            );
            var empty2MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: root2MatrixPosition.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 5,
                right: 6
            );
            var empty3MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: root3MatrixPosition.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 9,
                right: 10
            );
            var empty4MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: root3MatrixPosition.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 11,
                right: 12
            );

            modelBuilder.Entity<MatrixPosition>().HasData(
                root1MatrixPosition,
                root2MatrixPosition,
                root3MatrixPosition,
                empty1MatrixPosition,
                empty2MatrixPosition,
                empty3MatrixPosition,
                empty4MatrixPosition
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

            modelBuilder.Entity<PaymentHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.PaymentId).IsRequired();
                entity.Property(e => e.OrderId).IsRequired();
                entity.Property(e => e.AmountToPay).HasColumnType("decimal(18,6)");
                entity.Property(e => e.PaidAmount).HasColumnType("decimal(18,6)");
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.PaymentFor).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Text).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Number).ValueGeneratedOnAdd();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Subject).IsRequired();
                entity.Property(e => e.Text).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getutcdate()");
            });
        }
    }
}
