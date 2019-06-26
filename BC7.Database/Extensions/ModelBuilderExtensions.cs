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
            var root = new UserAccountData
            (
                Guid.NewGuid(),
                login: "root",
                email: "root@bitclub7.com",
                firstName: "FirstNameRoot",
                lastName: "LastNameRoot",
                street: "StreetRoot",
                city: "CityRoot",
                zipCode: "ZipCodeRoot",
                country: "CountryRoot",
                btcWalletAddress: "BtcWalletAddressRoot",
                role: UserRolesHelper.Root
            );
            root.SetPassword("salt1", "hash1");
            root.PaidMembershipFee();
            
            var admin = new UserAccountData
            (
                Guid.NewGuid(),
                login: "admin",
                email: "admin@bitclub7.com",
                firstName: "FirstNameAdmin",
                lastName: "LastNameAdmin",
                street: "StreetAdmin",
                city: "CityAdmin",
                zipCode: "ZipCodeAdmin",
                country: "CountryAdmin",
                btcWalletAddress: "BtcWalletAddressAdmin",
                role: UserRolesHelper.Admin
            );
            root.SetPassword("salt1", "hash1");
            root.PaidMembershipFee();

            modelBuilder.Entity<UserAccountData>().HasData(root, admin);


            var root1MultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: root.Id,
                sponsorId: null,
                multiAccountName: "root-001"
            );
            root1MultiAccount.SetAsMainAccount();

            var root2MultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: root.Id,
                sponsorId: null,
                multiAccountName: "root-002"
            );

            var root3MultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: root.Id,
                sponsorId: null,
                multiAccountName: "root-003"
            );

            var admin1MultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: admin.Id,
                sponsorId: null,
                multiAccountName: "admin-001"
            );
            admin1MultiAccount.SetAsMainAccount();

            var admin2MultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: admin.Id,
                sponsorId: null,
                multiAccountName: "admin-002"
            );

            var admin3MultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: admin.Id,
                sponsorId: null,
                multiAccountName: "admin-003"
            );

            var admin4MultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: admin.Id,
                sponsorId: null,
                multiAccountName: "admin-004"
            );

            modelBuilder.Entity<UserMultiAccount>().HasData(
                root1MultiAccount, root2MultiAccount, root3MultiAccount,
                admin1MultiAccount, admin2MultiAccount, admin3MultiAccount, admin4MultiAccount);


            var root1MatrixPosition = new MatrixPosition
            (
                // root-001
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
                // root-002
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
                // root-003
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: root1MatrixPosition.Id,
                userMultiAccountId: root3MultiAccount.Id,
                depthLevel: 1,
                left: 8,
                right: 13
            );
            // TODO: Admins in matrix on lvl0
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
