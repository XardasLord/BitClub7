﻿// <auto-generated />
using System;
using BC7.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BC7.Database.Migrations
{
    [DbContext(typeof(BitClub7Context))]
    [Migration("20190330130157_Seed_Database_With_Predefine_Root_Users")]
    partial class Seed_Database_With_Predefine_Root_Users
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BC7.Entity.MatrixPosition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("DepthLevel");

                    b.Property<int>("Left");

                    b.Property<int>("MatrixLevel");

                    b.Property<Guid?>("ParentId");

                    b.Property<int>("Right");

                    b.Property<Guid>("UserMultiAccountId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("UserMultiAccountId");

                    b.ToTable("MatrixPositions");
                });

            modelBuilder.Entity("BC7.Entity.UserAccountData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BtcWalletAddress")
                        .IsRequired();

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("Country")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("Hash")
                        .IsRequired();

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
                            Id = new Guid("b8f28295-923c-4616-aa46-81393ad7bdd2"),
                            BtcWalletAddress = "BtcWalletAddressRoot1",
                            City = "CityRoot1",
                            Country = "CountryRoot1",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "EmailRoot1",
                            FirstName = "FirstNameRoot1",
                            Hash = "",
                            LastName = "LastNameRoot1",
                            Login = "LoginRoot1",
                            Role = "Root",
                            Salt = "",
                            Street = "StreetRoot1",
                            ZipCode = "ZipCodeRoot1"
                        },
                        new
                        {
                            Id = new Guid("e71c10a5-bcb4-4ca8-9ab1-e60467baf44f"),
                            BtcWalletAddress = "BtcWalletAddressRoot2",
                            City = "CityRoot2",
                            Country = "CountryRoot2",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "EmailRoot2",
                            FirstName = "FirstNameRoot2",
                            Hash = "",
                            LastName = "LastNameRoot2",
                            Login = "LoginRoot2",
                            Role = "Root",
                            Salt = "",
                            Street = "StreetRoot2",
                            ZipCode = "ZipCodeRoot2"
                        },
                        new
                        {
                            Id = new Guid("2637699b-902d-4c13-a32b-0eb85ecdd25b"),
                            BtcWalletAddress = "BtcWalletAddressRoot3",
                            City = "CityRoot3",
                            Country = "CountryRoot3",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "EmailRoot3",
                            FirstName = "FirstNameRoot3",
                            Hash = "",
                            LastName = "LastNameRoot3",
                            Login = "LoginRoot3",
                            Role = "Root",
                            Salt = "",
                            Street = "StreetRoot3",
                            ZipCode = "ZipCodeRoot3"
                        });
                });

            modelBuilder.Entity("BC7.Entity.UserMultiAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("MultiAccountName");

                    b.Property<string>("RefLink");

                    b.Property<Guid>("UserAccountDataId");

                    b.Property<Guid?>("UserMultiAccountInvitingId");

                    b.HasKey("Id");

                    b.HasIndex("UserAccountDataId");

                    b.HasIndex("UserMultiAccountInvitingId");

                    b.ToTable("UserMultiAccounts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("18567dcb-042a-4e7a-b34b-539c12b04aa3"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MultiAccountName = "LoginRoot1",
                            RefLink = "111111",
                            UserAccountDataId = new Guid("b8f28295-923c-4616-aa46-81393ad7bdd2")
                        },
                        new
                        {
                            Id = new Guid("183bbc62-a871-488f-883d-970c09fb46ab"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MultiAccountName = "LoginRoot2",
                            RefLink = "222222",
                            UserAccountDataId = new Guid("e71c10a5-bcb4-4ca8-9ab1-e60467baf44f")
                        },
                        new
                        {
                            Id = new Guid("1dd37daf-cb83-44a4-8c2c-2dd85d519b77"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MultiAccountName = "LoginRoot3",
                            RefLink = "333333",
                            UserAccountDataId = new Guid("2637699b-902d-4c13-a32b-0eb85ecdd25b")
                        });
                });

            modelBuilder.Entity("BC7.Entity.MatrixPosition", b =>
                {
                    b.HasOne("BC7.Entity.MatrixPosition", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.HasOne("BC7.Entity.UserMultiAccount", "UserMultiAccount")
                        .WithMany()
                        .HasForeignKey("UserMultiAccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BC7.Entity.UserMultiAccount", b =>
                {
                    b.HasOne("BC7.Entity.UserAccountData", "UserAccountData")
                        .WithMany("UserMultiAccounts")
                        .HasForeignKey("UserAccountDataId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BC7.Entity.UserMultiAccount", "UserMultiAccountInviting")
                        .WithMany()
                        .HasForeignKey("UserMultiAccountInvitingId");
                });
#pragma warning restore 612, 618
        }
    }
}