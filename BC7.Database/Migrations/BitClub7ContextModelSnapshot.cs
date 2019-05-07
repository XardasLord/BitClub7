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
                });

            modelBuilder.Entity("BC7.Domain.PaymentHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AmountToPay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<Guid>("OrderId");

                    b.Property<decimal>("PaidAmount")
                        .HasColumnType("decimal(18,2)");

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
