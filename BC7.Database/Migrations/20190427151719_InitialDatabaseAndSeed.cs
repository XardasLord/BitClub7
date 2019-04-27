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
                    PaymentFor = table.Column<string>(nullable: false),
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
                    { new Guid("c7e0c57c-7ce6-424e-9984-18d738263ab2"), new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(2998), 2, 3, 0, new Guid("fc3da3e6-2207-435c-a037-6c8ece47362e"), 4, null },
                    { new Guid("ba4d343f-4670-41c8-a621-890eb098e79c"), new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(2998), 2, 5, 0, new Guid("fc3da3e6-2207-435c-a037-6c8ece47362e"), 6, null },
                    { new Guid("fda12c80-56d9-40b3-8f58-d7d2b8ebc955"), new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(3001), 2, 9, 0, new Guid("28100b1c-488d-4ca8-9f2f-4154c9b85f54"), 10, null },
                    { new Guid("4dffe139-3085-4359-9d9d-e71d8bba5305"), new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(3001), 2, 11, 0, new Guid("28100b1c-488d-4ca8-9f2f-4154c9b85f54"), 12, null }
                });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "CreatedAt", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[,]
                {
                    { new Guid("5e4b3502-62ac-4660-a62b-f7a50f8b9fa5"), "BtcWalletAddressRoot1", "CityRoot1", "CountryRoot1", new DateTime(2019, 4, 27, 15, 17, 19, 341, DateTimeKind.Utc).AddTicks(9847), "EmailRoot1", "FirstNameRoot1", "hash1", true, "LastNameRoot1", "LoginRoot1", "Root", "salt1", "StreetRoot1", "ZipCodeRoot1" },
                    { new Guid("83ad4d65-da83-459f-a3bb-80137686d9c5"), "BtcWalletAddressRoot2", "CityRoot2", "CountryRoot2", new DateTime(2019, 4, 27, 15, 17, 19, 342, DateTimeKind.Utc).AddTicks(985), "EmailRoot2", "FirstNameRoot2", "hash2", true, "LastNameRoot2", "LoginRoot2", "Root", "salt2", "StreetRoot2", "ZipCodeRoot2" },
                    { new Guid("ef77cfc9-b3b7-4e10-8d65-eb3a48845cf0"), "BtcWalletAddressRoot3", "CityRoot3", "CountryRoot3", new DateTime(2019, 4, 27, 15, 17, 19, 342, DateTimeKind.Utc).AddTicks(997), "EmailRoot3", "FirstNameRoot3", "hash3", true, "LastNameRoot3", "LoginRoot3", "Root", "salt3", "StreetRoot3", "ZipCodeRoot3" }
                });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("0e388ca9-b282-4b67-b4eb-c04ca4b0cbad"), new DateTime(2019, 4, 27, 15, 17, 19, 343, DateTimeKind.Utc).AddTicks(1912), true, "LoginRoot1", "111111", null, new Guid("5e4b3502-62ac-4660-a62b-f7a50f8b9fa5") });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("4131d163-8a5f-49a1-ac39-db211ee26bba"), new DateTime(2019, 4, 27, 15, 17, 19, 343, DateTimeKind.Utc).AddTicks(5558), true, "LoginRoot2", "222222", null, new Guid("83ad4d65-da83-459f-a3bb-80137686d9c5") });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("058e260d-f42d-4621-b6ed-b7945fa2f373"), new DateTime(2019, 4, 27, 15, 17, 19, 343, DateTimeKind.Utc).AddTicks(5587), true, "LoginRoot3", "333333", null, new Guid("ef77cfc9-b3b7-4e10-8d65-eb3a48845cf0") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("d6d21237-08ff-48f6-9558-4e0e456cc2a0"), new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(2129), 0, 1, 0, null, 14, new Guid("0e388ca9-b282-4b67-b4eb-c04ca4b0cbad") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("fc3da3e6-2207-435c-a037-6c8ece47362e"), new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(2984), 1, 2, 0, new Guid("d6d21237-08ff-48f6-9558-4e0e456cc2a0"), 7, new Guid("4131d163-8a5f-49a1-ac39-db211ee26bba") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("28100b1c-488d-4ca8-9f2f-4154c9b85f54"), new DateTime(2019, 4, 27, 15, 17, 19, 344, DateTimeKind.Utc).AddTicks(2995), 1, 8, 0, new Guid("d6d21237-08ff-48f6-9558-4e0e456cc2a0"), 13, new Guid("058e260d-f42d-4621-b6ed-b7945fa2f373") });

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
