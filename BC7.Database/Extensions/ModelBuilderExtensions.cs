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
            var hashSaltForRoot = Security.PasswordUtilities.PasswordEncryptionUtilities.GenerateSaltedHash("test$123");
            root.SetPassword(hashSaltForRoot.Salt, hashSaltForRoot.Hash);
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
            var hashSaltForAdmin = Security.PasswordUtilities.PasswordEncryptionUtilities.GenerateSaltedHash("test$123");
            admin.SetPassword(hashSaltForAdmin.Salt, hashSaltForAdmin.Hash);
            admin.PaidMembershipFee();

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
            admin1MultiAccount.SetReflink("xm3dgjTbckuxSfk0");
            admin1MultiAccount.SetAsMainAccount();

            var admin2MultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: admin.Id,
                sponsorId: null,
                multiAccountName: "admin-002"
            );
            admin2MultiAccount.SetReflink("CbJGE3bl65zWhUwK");

            var admin3MultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: admin.Id,
                sponsorId: null,
                multiAccountName: "admin-003"
            );
            admin3MultiAccount.SetReflink("nB1Mw99LCQuKXUY2");

            var admin4MultiAccount = new UserMultiAccount
            (
                id: Guid.NewGuid(),
                userAccountDataId: admin.Id,
                sponsorId: null,
                multiAccountName: "admin-004"
            );
            admin4MultiAccount.SetReflink("2ERrHKzmA7bigBeY");

            modelBuilder.Entity<UserMultiAccount>().HasData(
                root1MultiAccount, root2MultiAccount, root3MultiAccount,
                admin1MultiAccount, admin2MultiAccount, admin3MultiAccount, admin4MultiAccount);


            #region Seed Matrix Level 0
            var root1MatrixPosition = new MatrixPosition
            (
                // root-001
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: null,
                userMultiAccountId: root1MultiAccount.Id,
                depthLevel: 0,
                left: 1,
                right: 30
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
                right: 15
            );
            var root3MatrixPosition = new MatrixPosition
            (
                // root-003
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: root1MatrixPosition.Id,
                userMultiAccountId: root3MultiAccount.Id,
                depthLevel: 1,
                left: 16,
                right: 29
            );
            var admin1MatrixPosition = new MatrixPosition
            (
                // admin-001
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: root2MatrixPosition.Id,
                userMultiAccountId: admin1MultiAccount.Id,
                depthLevel: 2,
                left: 3,
                right: 8
            );
            var admin2MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: root2MatrixPosition.Id,
                userMultiAccountId: admin2MultiAccount.Id,
                depthLevel: 2,
                left: 9,
                right: 14
            );
            var admin3MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: root3MatrixPosition.Id,
                userMultiAccountId: admin3MultiAccount.Id,
                depthLevel: 2,
                left: 17,
                right: 22
            );
            var admin4MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: root3MatrixPosition.Id,
                userMultiAccountId: admin4MultiAccount.Id,
                depthLevel: 2,
                left: 23,
                right: 28
            );
            var empty1MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: admin1MatrixPosition.Id,
                userMultiAccountId: null,
                depthLevel: 3,
                left: 4,
                right: 5
            );
            var empty2MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: admin1MatrixPosition.Id,
                userMultiAccountId: null,
                depthLevel: 3,
                left: 6,
                right: 7
            );
            var empty3MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: admin2MatrixPosition.Id,
                userMultiAccountId: null,
                depthLevel: 3,
                left: 10,
                right: 11
            );
            var empty4MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: admin2MatrixPosition.Id,
                userMultiAccountId: null,
                depthLevel: 3,
                left: 12,
                right: 13
            );
            var empty5MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: admin3MatrixPosition.Id,
                userMultiAccountId: null,
                depthLevel: 3,
                left: 18,
                right: 19
            );
            var empty6MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: admin3MatrixPosition.Id,
                userMultiAccountId: null,
                depthLevel: 3,
                left: 20,
                right: 21
            );
            var empty7MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: admin4MatrixPosition.Id,
                userMultiAccountId: null,
                depthLevel: 3,
                left: 24,
                right: 25
            );
            var empty8MatrixPosition = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 0,
                parentId: admin4MatrixPosition.Id,
                userMultiAccountId: null,
                depthLevel: 3,
                left: 26,
                right: 27
            );

            modelBuilder.Entity<MatrixPosition>().HasData(
                root1MatrixPosition,
                root2MatrixPosition,
                root3MatrixPosition,
                admin1MatrixPosition,
                admin2MatrixPosition,
                admin3MatrixPosition,
                admin4MatrixPosition,
                empty1MatrixPosition,
                empty2MatrixPosition,
                empty3MatrixPosition,
                empty4MatrixPosition,
                empty5MatrixPosition,
                empty6MatrixPosition,
                empty7MatrixPosition,
                empty8MatrixPosition
            );
            #endregion
            #region Seed Matrix Level 1
            var root1MatrixPositionLvl1 = new MatrixPosition
            (
                // root-001
                id: Guid.NewGuid(),
                matrixLevel: 1,
                parentId: null,
                userMultiAccountId: root1MultiAccount.Id,
                depthLevel: 0,
                left: 1,
                right: 14
            );
            var root2MatrixPositionLvl1 = new MatrixPosition
            (
                // root-002
                id: Guid.NewGuid(),
                matrixLevel: 1,
                parentId: root1MatrixPositionLvl1.Id,
                userMultiAccountId: root2MultiAccount.Id,
                depthLevel: 1,
                left: 2,
                right: 7
            );
            var root3MatrixPositionLvl1 = new MatrixPosition
            (
                // root-003
                id: Guid.NewGuid(),
                matrixLevel: 1,
                parentId: root1MatrixPositionLvl1.Id,
                userMultiAccountId: root3MultiAccount.Id,
                depthLevel: 1,
                left: 8,
                right: 13
            );
            var empty1MatrixPositionLvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 1,
                parentId: root2MatrixPositionLvl1.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 3,
                right: 4
            );
            var empty2MatrixPositionLvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 1,
                parentId: root2MatrixPositionLvl1.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 5,
                right: 6
            );
            var empty3MatrixPositionLvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 1,
                parentId: root3MatrixPositionLvl1.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 9,
                right: 10
            );
            var empty4MatrixPositionLvl1 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 1,
                parentId: root3MatrixPositionLvl1.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 11,
                right: 12
            );

            modelBuilder.Entity<MatrixPosition>().HasData(
                root1MatrixPositionLvl1,
                root2MatrixPositionLvl1,
                root3MatrixPositionLvl1,
                empty1MatrixPositionLvl1,
                empty2MatrixPositionLvl1,
                empty3MatrixPositionLvl1,
                empty4MatrixPositionLvl1
            );
            #endregion
            #region Seed Matrix Level 2
            var root1MatrixPositionLvl2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 2,
                parentId: null,
                userMultiAccountId: root1MultiAccount.Id,
                depthLevel: 0,
                left: 1,
                right: 14
            );
            var root2MatrixPositionLvl2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 2,
                parentId: root1MatrixPositionLvl2.Id,
                userMultiAccountId: root2MultiAccount.Id,
                depthLevel: 1,
                left: 2,
                right: 7
            );
            var root3MatrixPositionLvl2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 2,
                parentId: root1MatrixPositionLvl2.Id,
                userMultiAccountId: root3MultiAccount.Id,
                depthLevel: 1,
                left: 8,
                right: 13
            );
            var empty1MatrixPositionLvl2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 2,
                parentId: root2MatrixPositionLvl2.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 3,
                right: 4
            );
            var empty2MatrixPositionLvl2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 2,
                parentId: root2MatrixPositionLvl2.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 5,
                right: 6
            );
            var empty3MatrixPositionLvl2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 2,
                parentId: root3MatrixPositionLvl2.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 9,
                right: 10
            );
            var empty4MatrixPositionLvl2 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 2,
                parentId: root3MatrixPositionLvl2.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 11,
                right: 12
            );

            modelBuilder.Entity<MatrixPosition>().HasData(
                root1MatrixPositionLvl2,
                root2MatrixPositionLvl2,
                root3MatrixPositionLvl2,
                empty1MatrixPositionLvl2,
                empty2MatrixPositionLvl2,
                empty3MatrixPositionLvl2,
                empty4MatrixPositionLvl2
            );
            #endregion
            #region Seed Matrix Level 3
            var root1MatrixPositionLvl3 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 3,
                parentId: null,
                userMultiAccountId: root1MultiAccount.Id,
                depthLevel: 0,
                left: 1,
                right: 14
            );
            var root2MatrixPositionLvl3 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 3,
                parentId: root1MatrixPositionLvl3.Id,
                userMultiAccountId: root2MultiAccount.Id,
                depthLevel: 1,
                left: 2,
                right: 7
            );
            var root3MatrixPositionLvl3 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 3,
                parentId: root1MatrixPositionLvl3.Id,
                userMultiAccountId: root3MultiAccount.Id,
                depthLevel: 1,
                left: 8,
                right: 13
            );
            var empty1MatrixPositionLvl3 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 3,
                parentId: root2MatrixPositionLvl3.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 3,
                right: 4
            );
            var empty2MatrixPositionLvl3 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 3,
                parentId: root2MatrixPositionLvl3.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 5,
                right: 6
            );
            var empty3MatrixPositionLvl3 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 3,
                parentId: root3MatrixPositionLvl3.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 9,
                right: 10
            );
            var empty4MatrixPositionLvl3 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 3,
                parentId: root3MatrixPositionLvl3.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 11,
                right: 12
            );

            modelBuilder.Entity<MatrixPosition>().HasData(
                root1MatrixPositionLvl3,
                root2MatrixPositionLvl3,
                root3MatrixPositionLvl3,
                empty1MatrixPositionLvl3,
                empty2MatrixPositionLvl3,
                empty3MatrixPositionLvl3,
                empty4MatrixPositionLvl3
            );
            #endregion
            #region Seed Matrix Level 4
            var root1MatrixPositionLvl4 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 4,
                parentId: null,
                userMultiAccountId: root1MultiAccount.Id,
                depthLevel: 0,
                left: 1,
                right: 14
            );
            var root2MatrixPositionLvl4 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 4,
                parentId: root1MatrixPositionLvl4.Id,
                userMultiAccountId: root2MultiAccount.Id,
                depthLevel: 1,
                left: 2,
                right: 7
            );
            var root3MatrixPositionLvl4 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 4,
                parentId: root1MatrixPositionLvl4.Id,
                userMultiAccountId: root3MultiAccount.Id,
                depthLevel: 1,
                left: 8,
                right: 13
            );
            var empty1MatrixPositionLvl4 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 4,
                parentId: root2MatrixPositionLvl4.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 3,
                right: 4
            );
            var empty2MatrixPositionLvl4 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 4,
                parentId: root2MatrixPositionLvl4.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 5,
                right: 6
            );
            var empty3MatrixPositionLvl4 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 4,
                parentId: root3MatrixPositionLvl4.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 9,
                right: 10
            );
            var empty4MatrixPositionLvl4 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 4,
                parentId: root3MatrixPositionLvl4.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 11,
                right: 12
            );

            modelBuilder.Entity<MatrixPosition>().HasData(
                root1MatrixPositionLvl4,
                root2MatrixPositionLvl4,
                root3MatrixPositionLvl4,
                empty1MatrixPositionLvl4,
                empty2MatrixPositionLvl4,
                empty3MatrixPositionLvl4,
                empty4MatrixPositionLvl4
            );
            #endregion
            #region Seed Matrix Level 5
            var root1MatrixPositionLvl5 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 5,
                parentId: null,
                userMultiAccountId: root1MultiAccount.Id,
                depthLevel: 0,
                left: 1,
                right: 14
            );
            var root2MatrixPositionLvl5 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 5,
                parentId: root1MatrixPositionLvl5.Id,
                userMultiAccountId: root2MultiAccount.Id,
                depthLevel: 1,
                left: 2,
                right: 7
            );
            var root3MatrixPositionLvl5 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 5,
                parentId: root1MatrixPositionLvl5.Id,
                userMultiAccountId: root3MultiAccount.Id,
                depthLevel: 1,
                left: 8,
                right: 13
            );
            var empty1MatrixPositionLvl5 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 5,
                parentId: root2MatrixPositionLvl5.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 3,
                right: 4
            );
            var empty2MatrixPositionLvl5 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 5,
                parentId: root2MatrixPositionLvl5.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 5,
                right: 6
            );
            var empty3MatrixPositionLvl5 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 5,
                parentId: root3MatrixPositionLvl5.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 9,
                right: 10
            );
            var empty4MatrixPositionLvl5 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 5,
                parentId: root3MatrixPositionLvl5.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 11,
                right: 12
            );

            modelBuilder.Entity<MatrixPosition>().HasData(
                root1MatrixPositionLvl5,
                root2MatrixPositionLvl5,
                root3MatrixPositionLvl5,
                empty1MatrixPositionLvl5,
                empty2MatrixPositionLvl5,
                empty3MatrixPositionLvl5,
                empty4MatrixPositionLvl5
            );
            #endregion
            #region Seed Matrix Level 6
            var root1MatrixPositionLvl6 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 6,
                parentId: null,
                userMultiAccountId: root1MultiAccount.Id,
                depthLevel: 0,
                left: 1,
                right: 14
            );
            var root2MatrixPositionLvl6 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 6,
                parentId: root1MatrixPositionLvl6.Id,
                userMultiAccountId: root2MultiAccount.Id,
                depthLevel: 1,
                left: 2,
                right: 7
            );
            var root3MatrixPositionLvl6 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 6,
                parentId: root1MatrixPositionLvl6.Id,
                userMultiAccountId: root3MultiAccount.Id,
                depthLevel: 1,
                left: 8,
                right: 13
            );
            var empty1MatrixPositionLvl6 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 6,
                parentId: root2MatrixPositionLvl6.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 3,
                right: 4
            );
            var empty2MatrixPositionLvl6 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 6,
                parentId: root2MatrixPositionLvl6.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 5,
                right: 6
            );
            var empty3MatrixPositionLvl6 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 6,
                parentId: root3MatrixPositionLvl6.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 9,
                right: 10
            );
            var empty4MatrixPositionLvl6 = new MatrixPosition
            (
                id: Guid.NewGuid(),
                matrixLevel: 6,
                parentId: root3MatrixPositionLvl6.Id,
                userMultiAccountId: null,
                depthLevel: 2,
                left: 11,
                right: 12
            );

            modelBuilder.Entity<MatrixPosition>().HasData(
                root1MatrixPositionLvl6,
                root2MatrixPositionLvl6,
                root3MatrixPositionLvl6,
                empty1MatrixPositionLvl6,
                empty2MatrixPositionLvl6,
                empty3MatrixPositionLvl6,
                empty4MatrixPositionLvl6
            );
            #endregion
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

            modelBuilder.Entity<Withdrawal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,6)");
                entity.Property(e => e.PaymentSystemType).IsRequired();
                entity.Property(e => e.UserMultiAccountId).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getutcdate()");
            });
        }
    }
}
