using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BC7.Database.Migrations
{
    public partial class InitialDatabaseAndSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PaymentId = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    AmountToPay = table.Column<double>(nullable: false),
                    PaidAmount = table.Column<double>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccountsData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Login = table.Column<string>(nullable: false),
                    Salt = table.Column<string>(nullable: false),
                    Hash = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Street = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: false),
                    BtcWalletAddress = table.Column<string>(nullable: false),
                    Role = table.Column<string>(nullable: false),
                    IsMembershipFeePaid = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccountsData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMultiAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserAccountDataId = table.Column<Guid>(nullable: false),
                    SponsorId = table.Column<Guid>(nullable: true),
                    MultiAccountName = table.Column<string>(nullable: true),
                    RefLink = table.Column<string>(nullable: true),
                    IsMainAccount = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMultiAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMultiAccounts_UserMultiAccounts_SponsorId",
                        column: x => x.SponsorId,
                        principalTable: "UserMultiAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserMultiAccounts_UserAccountsData_UserAccountDataId",
                        column: x => x.UserAccountDataId,
                        principalTable: "UserAccountsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatrixPositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserMultiAccountId = table.Column<Guid>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    MatrixLevel = table.Column<int>(nullable: false),
                    Left = table.Column<int>(nullable: false),
                    Right = table.Column<int>(nullable: false),
                    DepthLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatrixPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatrixPositions_UserMultiAccounts_UserMultiAccountId",
                        column: x => x.UserMultiAccountId,
                        principalTable: "UserMultiAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[,]
                {
                    { new Guid("94c88148-bed4-4e3a-98ef-66568517140b"), new DateTime(2019, 4, 24, 17, 51, 8, 459, DateTimeKind.Utc).AddTicks(9027), 2, 3, 0, new Guid("73ab5d25-0cd5-4e59-90b2-9235d93539cc"), 4, null },
                    { new Guid("d8dbe897-7f0a-44e7-ab05-4a37b7c688bc"), new DateTime(2019, 4, 24, 17, 51, 8, 459, DateTimeKind.Utc).AddTicks(9030), 2, 5, 0, new Guid("73ab5d25-0cd5-4e59-90b2-9235d93539cc"), 6, null },
                    { new Guid("650fa797-cc7d-4e02-bd4f-441723a9371e"), new DateTime(2019, 4, 24, 17, 51, 8, 459, DateTimeKind.Utc).AddTicks(9042), 2, 9, 0, new Guid("555377bd-2bc7-4e85-b7a9-8f921ac60fb2"), 10, null },
                    { new Guid("f340082f-f501-4b51-b1db-60059d42dedf"), new DateTime(2019, 4, 24, 17, 51, 8, 459, DateTimeKind.Utc).AddTicks(9042), 2, 11, 0, new Guid("555377bd-2bc7-4e85-b7a9-8f921ac60fb2"), 12, null }
                });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "CreatedAt", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[,]
                {
                    { new Guid("40390b5a-be5a-49f9-8b8e-1d77903f5f89"), "BtcWalletAddressRoot1", "CityRoot1", "CountryRoot1", new DateTime(2019, 4, 24, 17, 51, 8, 457, DateTimeKind.Utc).AddTicks(3161), "EmailRoot1", "FirstNameRoot1", "hash1", true, "LastNameRoot1", "LoginRoot1", "Root", "salt1", "StreetRoot1", "ZipCodeRoot1" },
                    { new Guid("1d1d72f6-4f6e-4e87-872a-66042b0d17e5"), "BtcWalletAddressRoot2", "CityRoot2", "CountryRoot2", new DateTime(2019, 4, 24, 17, 51, 8, 457, DateTimeKind.Utc).AddTicks(4431), "EmailRoot2", "FirstNameRoot2", "hash2", true, "LastNameRoot2", "LoginRoot2", "Root", "salt2", "StreetRoot2", "ZipCodeRoot2" },
                    { new Guid("38e6fba6-23c5-410e-a478-010e9a253cfc"), "BtcWalletAddressRoot3", "CityRoot3", "CountryRoot3", new DateTime(2019, 4, 24, 17, 51, 8, 457, DateTimeKind.Utc).AddTicks(4443), "EmailRoot3", "FirstNameRoot3", "hash3", true, "LastNameRoot3", "LoginRoot3", "Root", "salt3", "StreetRoot3", "ZipCodeRoot3" }
                });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("91e63abf-2417-45d8-b570-1aa06b3d2fcd"), new DateTime(2019, 4, 24, 17, 51, 8, 458, DateTimeKind.Utc).AddTicks(4847), true, "LoginRoot1", "111111", null, new Guid("40390b5a-be5a-49f9-8b8e-1d77903f5f89") });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("779083e7-83bf-4202-9748-55f21d2c83a8"), new DateTime(2019, 4, 24, 17, 51, 8, 459, DateTimeKind.Utc).AddTicks(311), true, "LoginRoot2", "222222", null, new Guid("1d1d72f6-4f6e-4e87-872a-66042b0d17e5") });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("35476f62-4d2c-4aa6-b449-6d422d14001b"), new DateTime(2019, 4, 24, 17, 51, 8, 459, DateTimeKind.Utc).AddTicks(332), true, "LoginRoot3", "333333", null, new Guid("38e6fba6-23c5-410e-a478-010e9a253cfc") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("23eb9ca7-8861-4ddd-a332-f611eb7ddb29"), new DateTime(2019, 4, 24, 17, 51, 8, 459, DateTimeKind.Utc).AddTicks(8135), 0, 1, 0, null, 14, new Guid("91e63abf-2417-45d8-b570-1aa06b3d2fcd") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("73ab5d25-0cd5-4e59-90b2-9235d93539cc"), new DateTime(2019, 4, 24, 17, 51, 8, 459, DateTimeKind.Utc).AddTicks(9015), 1, 2, 0, new Guid("23eb9ca7-8861-4ddd-a332-f611eb7ddb29"), 7, new Guid("779083e7-83bf-4202-9748-55f21d2c83a8") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("555377bd-2bc7-4e85-b7a9-8f921ac60fb2"), new DateTime(2019, 4, 24, 17, 51, 8, 459, DateTimeKind.Utc).AddTicks(9027), 1, 8, 0, new Guid("23eb9ca7-8861-4ddd-a332-f611eb7ddb29"), 13, new Guid("35476f62-4d2c-4aa6-b449-6d422d14001b") });

            migrationBuilder.CreateIndex(
                name: "IX_MatrixPositions_UserMultiAccountId",
                table: "MatrixPositions",
                column: "UserMultiAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountsData_Email",
                table: "UserAccountsData",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountsData_Login",
                table: "UserAccountsData",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMultiAccounts_SponsorId",
                table: "UserMultiAccounts",
                column: "SponsorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMultiAccounts_UserAccountDataId",
                table: "UserMultiAccounts",
                column: "UserAccountDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatrixPositions");

            migrationBuilder.DropTable(
                name: "PaymentHistories");

            migrationBuilder.DropTable(
                name: "UserMultiAccounts");

            migrationBuilder.DropTable(
                name: "UserAccountsData");
        }
    }
}
