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
                    AmountToPay = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
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
                    { new Guid("0576e73b-d29a-4ec8-8b46-cb80a5c1d6a8"), new DateTime(2019, 5, 7, 17, 8, 46, 570, DateTimeKind.Utc).AddTicks(4887), 2, 3, 0, new Guid("ac0d3b05-11a0-438a-a4f3-9efca4f843df"), 4, null },
                    { new Guid("70502fd0-bf66-4455-984a-951b12aae788"), new DateTime(2019, 5, 7, 17, 8, 46, 570, DateTimeKind.Utc).AddTicks(4887), 2, 5, 0, new Guid("ac0d3b05-11a0-438a-a4f3-9efca4f843df"), 6, null },
                    { new Guid("21593e16-ea25-44a0-b0c4-e4236d8f62c6"), new DateTime(2019, 5, 7, 17, 8, 46, 570, DateTimeKind.Utc).AddTicks(4934), 2, 9, 0, new Guid("a3f3169a-3e6e-4558-a7da-99829c974fb3"), 10, null },
                    { new Guid("b8d6f888-13c8-4109-95aa-83ce4dce4499"), new DateTime(2019, 5, 7, 17, 8, 46, 570, DateTimeKind.Utc).AddTicks(4937), 2, 11, 0, new Guid("a3f3169a-3e6e-4558-a7da-99829c974fb3"), 12, null }
                });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "CreatedAt", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[,]
                {
                    { new Guid("228673ef-51ec-4b82-a1a1-76ea020a84be"), "BtcWalletAddressRoot1", "CityRoot1", "CountryRoot1", new DateTime(2019, 5, 7, 17, 8, 46, 568, DateTimeKind.Utc).AddTicks(3137), "EmailRoot1", "FirstNameRoot1", "hash1", true, "LastNameRoot1", "LoginRoot1", "Root", "salt1", "StreetRoot1", "ZipCodeRoot1" },
                    { new Guid("813c380c-8c3b-473e-9a50-c20fd0042822"), "BtcWalletAddressRoot2", "CityRoot2", "CountryRoot2", new DateTime(2019, 5, 7, 17, 8, 46, 568, DateTimeKind.Utc).AddTicks(4296), "EmailRoot2", "FirstNameRoot2", "hash2", true, "LastNameRoot2", "LoginRoot2", "Root", "salt2", "StreetRoot2", "ZipCodeRoot2" },
                    { new Guid("bff1a558-f396-4516-a950-1df026c4ec0e"), "BtcWalletAddressRoot3", "CityRoot3", "CountryRoot3", new DateTime(2019, 5, 7, 17, 8, 46, 568, DateTimeKind.Utc).AddTicks(4310), "EmailRoot3", "FirstNameRoot3", "hash3", true, "LastNameRoot3", "LoginRoot3", "Root", "salt3", "StreetRoot3", "ZipCodeRoot3" }
                });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("1c70cd91-91b9-426d-86be-55a2b5e95d55"), new DateTime(2019, 5, 7, 17, 8, 46, 569, DateTimeKind.Utc).AddTicks(3310), true, "LoginRoot1", "111111", null, new Guid("228673ef-51ec-4b82-a1a1-76ea020a84be") });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("a13f108e-2ce8-4ff0-a80b-ba1f9c0b424b"), new DateTime(2019, 5, 7, 17, 8, 46, 569, DateTimeKind.Utc).AddTicks(6976), true, "LoginRoot2", "222222", null, new Guid("813c380c-8c3b-473e-9a50-c20fd0042822") });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("d2c3d95e-9d0e-4420-b120-0c2b412a406a"), new DateTime(2019, 5, 7, 17, 8, 46, 569, DateTimeKind.Utc).AddTicks(6997), true, "LoginRoot3", "333333", null, new Guid("bff1a558-f396-4516-a950-1df026c4ec0e") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("30d1fa80-ec36-40ea-8294-4aff3908d6e3"), new DateTime(2019, 5, 7, 17, 8, 46, 570, DateTimeKind.Utc).AddTicks(3992), 0, 1, 0, null, 14, new Guid("1c70cd91-91b9-426d-86be-55a2b5e95d55") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("ac0d3b05-11a0-438a-a4f3-9efca4f843df"), new DateTime(2019, 5, 7, 17, 8, 46, 570, DateTimeKind.Utc).AddTicks(4873), 1, 2, 0, new Guid("30d1fa80-ec36-40ea-8294-4aff3908d6e3"), 7, new Guid("a13f108e-2ce8-4ff0-a80b-ba1f9c0b424b") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("a3f3169a-3e6e-4558-a7da-99829c974fb3"), new DateTime(2019, 5, 7, 17, 8, 46, 570, DateTimeKind.Utc).AddTicks(4885), 1, 8, 0, new Guid("30d1fa80-ec36-40ea-8294-4aff3908d6e3"), 13, new Guid("d2c3d95e-9d0e-4420-b120-0c2b412a406a") });

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
