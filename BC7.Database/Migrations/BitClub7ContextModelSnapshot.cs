﻿// <auto-generated />
using System;
using BC7.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BC7.Database.Migrations
{
    [DbContext(typeof(BitClub7Context))]
    partial class BitClub7ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BC7.Domain.MatrixPosition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<int>("DepthLevel");

                    b.Property<int>("Left");

                    b.Property<int>("MatrixLevel");

                    b.Property<Guid?>("ParentId");

                    b.Property<int>("Right");

                    b.Property<Guid?>("UserMultiAccountId");

                    b.HasKey("Id");

                    b.HasIndex("UserMultiAccountId");

                    b.ToTable("MatrixPositions");

                    b.HasData(
                        new
                        {
                            Id = new Guid("d6d21237-08ff-48f6-9558-4e0e456cc2a0"),
                            CreatedAt = new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(2129),
                            DepthLevel = 0,
                            Left = 1,
                            MatrixLevel = 0,
                            Right = 14,
                            UserMultiAccountId = new Guid("0e388ca9-b282-4b67-b4eb-c04ca4b0cbad")
                        },
                        new
                        {
                            Id = new Guid("fc3da3e6-2207-435c-a037-6c8ece47362e"),
                            CreatedAt = new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(2984),
                            DepthLevel = 1,
                            Left = 2,
                            MatrixLevel = 0,
                            ParentId = new Guid("d6d21237-08ff-48f6-9558-4e0e456cc2a0"),
                            Right = 7,
                            UserMultiAccountId = new Guid("4131d163-8a5f-49a1-ac39-db211ee26bba")
                        },
                        new
                        {
                            Id = new Guid("28100b1c-488d-4ca8-9f2f-4154c9b85f54"),
                            CreatedAt = new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(2995),
                            DepthLevel = 1,
                            Left = 8,
                            MatrixLevel = 0,
                            ParentId = new Guid("d6d21237-08ff-48f6-9558-4e0e456cc2a0"),
                            Right = 13,
                            UserMultiAccountId = new Guid("058e260d-f42d-4621-b6ed-b7945fa2f373")
                        },
                        new
                        {
                            Id = new Guid("c7e0c57c-7ce6-424e-9984-18d738263ab2"),
                            CreatedAt = new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(2998),
                            DepthLevel = 2,
                            Left = 3,
                            MatrixLevel = 0,
                            ParentId = new Guid("fc3da3e6-2207-435c-a037-6c8ece47362e"),
                            Right = 4
                        },
                        new
                        {
                            Id = new Guid("ba4d343f-4670-41c8-a621-890eb098e79c"),
                            CreatedAt = new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(2998),
                            DepthLevel = 2,
                            Left = 5,
                            MatrixLevel = 0,
                            ParentId = new Guid("fc3da3e6-2207-435c-a037-6c8ece47362e"),
                            Right = 6
                        },
                        new
                        {
                            Id = new Guid("fda12c80-56d9-40b3-8f58-d7d2b8ebc955"),
                            CreatedAt = new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(3001),
                            DepthLevel = 2,
                            Left = 9,
                            MatrixLevel = 0,
                            ParentId = new Guid("28100b1c-488d-4ca8-9f2f-4154c9b85f54"),
                            Right = 10
                        },
                        new
                        {
                            Id = new Guid("4dffe139-3085-4359-9d9d-e71d8bba5305"),
                            CreatedAt = new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(3001),
                            DepthLevel = 2,
                            Left = 11,
                            MatrixLevel = 0,
                            ParentId = new Guid("28100b1c-488d-4ca8-9f2f-4154c9b85f54"),
                            Right = 12
                        });
                });

            modelBuilder.Entity("BC7.Domain.PaymentHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("AmountToPay");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<Guid>("OrderId");

                    b.Property<double>("PaidAmount");

                    b.Property<string>("PaymentFor")
                        .IsRequired();

                    b.Property<Guid>("PaymentId");

                    b.Property<string>("Status")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("PaymentHistories");
                });

            modelBuilder.Entity("BC7.Domain.UserAccountData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BtcWalletAddress")
                        .IsRequired();

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("Country")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("Hash")
                        .IsRequired();

                    b.Property<bool>("IsMembershipFeePaid");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Login")
                        .IsRequired();

                    b.Property<string>("Role")
                        .IsRequired();

                    b.Property<string>("Salt")
                        .IsRequired();

                    b.Property<string>("Street")
                        .IsRequired();

                    b.Property<string>("ZipCode")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("UserAccountsData");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5e4b3502-62ac-4660-a62b-f7a50f8b9fa5"),
                            BtcWalletAddress = "BtcWalletAddressRoot1",
                            City = "CityRoot1",
                            Country = "CountryRoot1",
                            CreatedAt = new DateTime(2019, 4, 27, 15, 17, 19, 341, DateTimeKind.Utc).AddTicks(9847),
                            Email = "EmailRoot1",
                            FirstName = "FirstNameRoot1",
                            Hash = "hash1",
                            IsMembershipFeePaid = true,
                            LastName = "LastNameRoot1",
                            Login = "LoginRoot1",
                            Role = "Root",
                            Salt = "salt1",
                            Street = "StreetRoot1",
                            ZipCode = "ZipCodeRoot1"
                        },
                        new
                        {
                            Id = new Guid("83ad4d65-da83-459f-a3bb-80137686d9c5"),
                            BtcWalletAddress = "BtcWalletAddressRoot2",
                            City = "CityRoot2",
                            Country = "CountryRoot2",
                            CreatedAt = new DateTime(2019, 4, 27, 15, 17, 19, 342, DateTimeKind.Utc).AddTicks(985),
                            Email = "EmailRoot2",
                            FirstName = "FirstNameRoot2",
                            Hash = "hash2",
                            IsMembershipFeePaid = true,
                            LastName = "LastNameRoot2",
                            Login = "LoginRoot2",
                            Role = "Root",
                            Salt = "salt2",
                            Street = "StreetRoot2",
                            ZipCode = "ZipCodeRoot2"
                        },
                        new
                        {
                            Id = new Guid("ef77cfc9-b3b7-4e10-8d65-eb3a48845cf0"),
                            BtcWalletAddress = "BtcWalletAddressRoot3",
                            City = "CityRoot3",
                            Country = "CountryRoot3",
                            CreatedAt = new DateTime(2019, 4, 27, 15, 17, 19, 342, DateTimeKind.Utc).AddTicks(997),
                            Email = "EmailRoot3",
                            FirstName = "FirstNameRoot3",
                            Hash = "hash3",
                            IsMembershipFeePaid = true,
                            LastName = "LastNameRoot3",
                            Login = "LoginRoot3",
                            Role = "Root",
                            Salt = "salt3",
                            Street = "StreetRoot3",
                            ZipCode = "ZipCodeRoot3"
                        });
                });

            modelBuilder.Entity("BC7.Domain.UserMultiAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<bool>("IsMainAccount");

                    b.Property<string>("MultiAccountName");

                    b.Property<string>("RefLink");

                    b.Property<Guid?>("SponsorId");

                    b.Property<Guid>("UserAccountDataId");

                    b.HasKey("Id");

                    b.HasIndex("SponsorId");

                    b.HasIndex("UserAccountDataId");

                    b.ToTable("UserMultiAccounts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0e388ca9-b282-4b67-b4eb-c04ca4b0cbad"),
                            CreatedAt = new DateTime(2019, 4, 27, 15, 17, 19, 343, DateTimeKind.Utc).AddTicks(1912),
                            IsMainAccount = true,
                            MultiAccountName = "LoginRoot1",
                            RefLink = "111111",
                            UserAccountDataId = new Guid("5e4b3502-62ac-4660-a62b-f7a50f8b9fa5")
                        },
                        new
                        {
                            Id = new Guid("4131d163-8a5f-49a1-ac39-db211ee26bba"),
                            CreatedAt = new DateTime(2019, 4, 27, 15, 17, 19, 343, DateTimeKind.Utc).AddTicks(5558),
                            IsMainAccount = true,
                            MultiAccountName = "LoginRoot2",
                            RefLink = "222222",
                            UserAccountDataId = new Guid("83ad4d65-da83-459f-a3bb-80137686d9c5")
                        },
                        new
                        {
                            Id = new Guid("058e260d-f42d-4621-b6ed-b7945fa2f373"),
                            CreatedAt = new DateTime(2019, 4, 27, 15, 17, 19, 343, DateTimeKind.Utc).AddTicks(5587),
                            IsMainAccount = true,
                            MultiAccountName = "LoginRoot3",
                            RefLink = "333333",
                            UserAccountDataId = new Guid("ef77cfc9-b3b7-4e10-8d65-eb3a48845cf0")
                        });
                });

            modelBuilder.Entity("BC7.Domain.MatrixPosition", b =>
                {
                    b.HasOne("BC7.Domain.UserMultiAccount")
                        .WithMany("MatrixPositions")
                        .HasForeignKey("UserMultiAccountId");
                });

            modelBuilder.Entity("BC7.Domain.UserMultiAccount", b =>
                {
                    b.HasOne("BC7.Domain.UserMultiAccount", "Sponsor")
                        .WithMany()
                        .HasForeignKey("SponsorId");

                    b.HasOne("BC7.Domain.UserAccountData", "UserAccountData")
                        .WithMany("UserMultiAccounts")
                        .HasForeignKey("UserAccountDataId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
