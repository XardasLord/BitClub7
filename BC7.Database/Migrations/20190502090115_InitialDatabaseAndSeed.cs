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
                    AmountToPay = table.Column<decimal>(nullable: false),
                    PaidAmount = table.Column<decimal>(nullable: false),
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
                    { new Guid("ec737a2c-36a9-40d8-a2d7-7f6208a2d42b"), new DateTime(2019, 5, 2, 9, 1, 14, 829, DateTimeKind.Utc).AddTicks(9822), 2, 3, 0, new Guid("6376eb00-cde1-4cc6-bfb9-ac9a6c810a34"), 4, null },
                    { new Guid("22df768d-29a0-4624-b02e-199d928325ac"), new DateTime(2019, 5, 2, 9, 1, 14, 829, DateTimeKind.Utc).AddTicks(9822), 2, 5, 0, new Guid("6376eb00-cde1-4cc6-bfb9-ac9a6c810a34"), 6, null },
                    { new Guid("6589418f-225d-4039-bd4d-c7602a887ab0"), new DateTime(2019, 5, 2, 9, 1, 14, 829, DateTimeKind.Utc).AddTicks(9825), 2, 9, 0, new Guid("eb30aa94-e9fb-40ca-af04-72a0a952ab05"), 10, null },
                    { new Guid("cca19321-a65f-4fb1-8409-3e5d7c378cf1"), new DateTime(2019, 5, 2, 9, 1, 14, 829, DateTimeKind.Utc).AddTicks(9825), 2, 11, 0, new Guid("eb30aa94-e9fb-40ca-af04-72a0a952ab05"), 12, null }
                });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "CreatedAt", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[,]
                {
                    { new Guid("dc7b7925-207f-4357-9642-e97ad59eff63"), "BtcWalletAddressRoot1", "CityRoot1", "CountryRoot1", new DateTime(2019, 5, 2, 9, 1, 14, 827, DateTimeKind.Utc).AddTicks(3646), "EmailRoot1", "FirstNameRoot1", "hash1", true, "LastNameRoot1", "LoginRoot1", "Root", "salt1", "StreetRoot1", "ZipCodeRoot1" },
                    { new Guid("609dc926-7f81-4926-95fc-20a5f2bb75c7"), "BtcWalletAddressRoot2", "CityRoot2", "CountryRoot2", new DateTime(2019, 5, 2, 9, 1, 14, 827, DateTimeKind.Utc).AddTicks(4825), "EmailRoot2", "FirstNameRoot2", "hash2", true, "LastNameRoot2", "LoginRoot2", "Root", "salt2", "StreetRoot2", "ZipCodeRoot2" },
                    { new Guid("0185817d-771a-40d8-b4fd-f18ea7a1fe44"), "BtcWalletAddressRoot3", "CityRoot3", "CountryRoot3", new DateTime(2019, 5, 2, 9, 1, 14, 827, DateTimeKind.Utc).AddTicks(4837), "EmailRoot3", "FirstNameRoot3", "hash3", true, "LastNameRoot3", "LoginRoot3", "Root", "salt3", "StreetRoot3", "ZipCodeRoot3" }
                });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("a922136f-dc35-4f1c-aeb4-ace266614f46"), new DateTime(2019, 5, 2, 9, 1, 14, 828, DateTimeKind.Utc).AddTicks(6074), true, "LoginRoot1", "111111", null, new Guid("dc7b7925-207f-4357-9642-e97ad59eff63") });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("8e9d0086-1564-4fc1-9e4f-921355aa90e9"), new DateTime(2019, 5, 2, 9, 1, 14, 829, DateTimeKind.Utc).AddTicks(1867), true, "LoginRoot2", "222222", null, new Guid("609dc926-7f81-4926-95fc-20a5f2bb75c7") });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("976cd947-5911-42e7-9b96-18a699fa0241"), new DateTime(2019, 5, 2, 9, 1, 14, 829, DateTimeKind.Utc).AddTicks(1887), true, "LoginRoot3", "333333", null, new Guid("0185817d-771a-40d8-b4fd-f18ea7a1fe44") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("f1eae29d-8fa4-4a8d-bfb6-e565951d3a12"), new DateTime(2019, 5, 2, 9, 1, 14, 829, DateTimeKind.Utc).AddTicks(8842), 0, 1, 0, null, 14, new Guid("a922136f-dc35-4f1c-aeb4-ace266614f46") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("6376eb00-cde1-4cc6-bfb9-ac9a6c810a34"), new DateTime(2019, 5, 2, 9, 1, 14, 829, DateTimeKind.Utc).AddTicks(9717), 1, 2, 0, new Guid("f1eae29d-8fa4-4a8d-bfb6-e565951d3a12"), 7, new Guid("8e9d0086-1564-4fc1-9e4f-921355aa90e9") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("eb30aa94-e9fb-40ca-af04-72a0a952ab05"), new DateTime(2019, 5, 2, 9, 1, 14, 829, DateTimeKind.Utc).AddTicks(9811), 1, 8, 0, new Guid("f1eae29d-8fa4-4a8d-bfb6-e565951d3a12"), 13, new Guid("976cd947-5911-42e7-9b96-18a699fa0241") });

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
