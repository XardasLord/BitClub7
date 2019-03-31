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

            modelBuilder.Entity("BC7.Entity.MatrixPosition", b =>
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

                    b.HasIndex("ParentId");

                    b.HasIndex("UserMultiAccountId");

                    b.ToTable("MatrixPositions");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f02338ac-0bd9-48ce-af26-70229e5ebb94"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepthLevel = 0,
                            Left = 1,
                            MatrixLevel = 0,
                            Right = 14,
                            UserMultiAccountId = new Guid("a36071ce-0849-42bd-933b-14fb6cc2175a")
                        },
                        new
                        {
                            Id = new Guid("1af2f050-ae6e-4749-9419-d20b61c8f794"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepthLevel = 1,
                            Left = 2,
                            MatrixLevel = 0,
                            ParentId = new Guid("f02338ac-0bd9-48ce-af26-70229e5ebb94"),
                            Right = 7,
                            UserMultiAccountId = new Guid("6a2df1e2-3553-4f81-b73d-5ac257b00dbf")
                        },
                        new
                        {
                            Id = new Guid("d445b122-047b-444c-9238-c61fb2c28dda"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepthLevel = 1,
                            Left = 8,
                            MatrixLevel = 0,
                            ParentId = new Guid("f02338ac-0bd9-48ce-af26-70229e5ebb94"),
                            Right = 13,
                            UserMultiAccountId = new Guid("bda0b804-4d3b-41eb-aed6-cce41ce53945")
                        },
                        new
                        {
                            Id = new Guid("181a0761-ed8f-4c3b-993e-f69e6390cfbf"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepthLevel = 2,
                            Left = 3,
                            MatrixLevel = 0,
                            ParentId = new Guid("1af2f050-ae6e-4749-9419-d20b61c8f794"),
                            Right = 4
                        },
                        new
                        {
                            Id = new Guid("bf75e510-5f20-43ed-b693-e5d9c5fefcbb"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepthLevel = 2,
                            Left = 5,
                            MatrixLevel = 0,
                            ParentId = new Guid("1af2f050-ae6e-4749-9419-d20b61c8f794"),
                            Right = 6
                        },
                        new
                        {
                            Id = new Guid("975da247-9300-4afe-a70b-fdf0d342d295"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepthLevel = 2,
                            Left = 9,
                            MatrixLevel = 0,
                            ParentId = new Guid("d445b122-047b-444c-9238-c61fb2c28dda"),
                            Right = 10
                        },
                        new
                        {
                            Id = new Guid("1c7f1963-4cf6-4430-897b-dd79521e6755"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DepthLevel = 2,
                            Left = 11,
                            MatrixLevel = 0,
                            ParentId = new Guid("d445b122-047b-444c-9238-c61fb2c28dda"),
                            Right = 12
                        });
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
                            Id = new Guid("b40cb1fd-abe9-4cde-9fd9-e0a63f54f817"),
                            BtcWalletAddress = "BtcWalletAddressRoot1",
                            City = "CityRoot1",
                            Country = "CountryRoot1",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "EmailRoot1",
                            FirstName = "FirstNameRoot1",
                            Hash = "",
                            IsMembershipFeePaid = true,
                            LastName = "LastNameRoot1",
                            Login = "LoginRoot1",
                            Role = "Root",
                            Salt = "",
                            Street = "StreetRoot1",
                            ZipCode = "ZipCodeRoot1"
                        },
                        new
                        {
                            Id = new Guid("6d338237-3001-45b4-bed9-4f45356cb238"),
                            BtcWalletAddress = "BtcWalletAddressRoot2",
                            City = "CityRoot2",
                            Country = "CountryRoot2",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "EmailRoot2",
                            FirstName = "FirstNameRoot2",
                            Hash = "",
                            IsMembershipFeePaid = true,
                            LastName = "LastNameRoot2",
                            Login = "LoginRoot2",
                            Role = "Root",
                            Salt = "",
                            Street = "StreetRoot2",
                            ZipCode = "ZipCodeRoot2"
                        },
                        new
                        {
                            Id = new Guid("ee67e7e5-5169-4051-a0e5-577d3d1f1dcf"),
                            BtcWalletAddress = "BtcWalletAddressRoot3",
                            City = "CityRoot3",
                            Country = "CountryRoot3",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "EmailRoot3",
                            FirstName = "FirstNameRoot3",
                            Hash = "",
                            IsMembershipFeePaid = true,
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

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<bool>("IsMainAccount");

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
                            Id = new Guid("a36071ce-0849-42bd-933b-14fb6cc2175a"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsMainAccount = true,
                            MultiAccountName = "LoginRoot1",
                            RefLink = "111111",
                            UserAccountDataId = new Guid("b40cb1fd-abe9-4cde-9fd9-e0a63f54f817")
                        },
                        new
                        {
                            Id = new Guid("6a2df1e2-3553-4f81-b73d-5ac257b00dbf"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsMainAccount = true,
                            MultiAccountName = "LoginRoot2",
                            RefLink = "222222",
                            UserAccountDataId = new Guid("6d338237-3001-45b4-bed9-4f45356cb238")
                        },
                        new
                        {
                            Id = new Guid("bda0b804-4d3b-41eb-aed6-cce41ce53945"),
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsMainAccount = true,
                            MultiAccountName = "LoginRoot3",
                            RefLink = "333333",
                            UserAccountDataId = new Guid("ee67e7e5-5169-4051-a0e5-577d3d1f1dcf")
                        });
                });

            modelBuilder.Entity("BC7.Entity.MatrixPosition", b =>
                {
                    b.HasOne("BC7.Entity.MatrixPosition", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.HasOne("BC7.Entity.UserMultiAccount", "UserMultiAccount")
                        .WithMany("MatrixPositions")
                        .HasForeignKey("UserMultiAccountId");
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
