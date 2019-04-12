using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BC7.Database.Migrations
{
    public partial class InitialDatabaseAndSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { new Guid("9f0d2984-796e-444d-9bb1-3a403563d9d7"), new DateTime(2019, 4, 12, 13, 27, 18, 620, DateTimeKind.Utc).AddTicks(6251), 2, 3, 0, new Guid("1aba1a60-41d0-4812-b9f2-51faa2ec60d8"), 4, null },
                    { new Guid("f7b10f15-c7ec-4a0f-b7d2-c4b535b70681"), new DateTime(2019, 4, 12, 13, 27, 18, 620, DateTimeKind.Utc).AddTicks(6254), 2, 5, 0, new Guid("1aba1a60-41d0-4812-b9f2-51faa2ec60d8"), 6, null },
                    { new Guid("f9620432-d5fd-493c-b639-7e3a0b690fb4"), new DateTime(2019, 4, 12, 13, 27, 18, 620, DateTimeKind.Utc).AddTicks(6254), 2, 9, 0, new Guid("f1e4ba54-5d4d-47fa-8481-e790db06bf58"), 10, null },
                    { new Guid("250363c2-93b2-4f2b-bc5d-dde24b5a785e"), new DateTime(2019, 4, 12, 13, 27, 18, 620, DateTimeKind.Utc).AddTicks(6257), 2, 11, 0, new Guid("f1e4ba54-5d4d-47fa-8481-e790db06bf58"), 12, null }
                });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "CreatedAt", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[,]
                {
                    { new Guid("20ce71dc-f322-4d31-a525-d567ec0e2c9a"), "BtcWalletAddressRoot1", "CityRoot1", "CountryRoot1", new DateTime(2019, 4, 12, 13, 27, 18, 618, DateTimeKind.Utc).AddTicks(4071), "EmailRoot1", "FirstNameRoot1", "hash1", true, "LastNameRoot1", "LoginRoot1", "Root", "salt1", "StreetRoot1", "ZipCodeRoot1" },
                    { new Guid("f8319492-31b4-4b44-a750-667838a114ee"), "BtcWalletAddressRoot2", "CityRoot2", "CountryRoot2", new DateTime(2019, 4, 12, 13, 27, 18, 618, DateTimeKind.Utc).AddTicks(5312), "EmailRoot2", "FirstNameRoot2", "hash2", true, "LastNameRoot2", "LoginRoot2", "Root", "salt2", "StreetRoot2", "ZipCodeRoot2" },
                    { new Guid("0ea1c2ff-b944-4aa8-98ec-a967cf7d7a65"), "BtcWalletAddressRoot3", "CityRoot3", "CountryRoot3", new DateTime(2019, 4, 12, 13, 27, 18, 618, DateTimeKind.Utc).AddTicks(5324), "EmailRoot3", "FirstNameRoot3", "hash3", true, "LastNameRoot3", "LoginRoot3", "Root", "salt3", "StreetRoot3", "ZipCodeRoot3" }
                });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("07d685e1-bed9-4598-8952-5a111f6fa15d"), new DateTime(2019, 4, 12, 13, 27, 18, 619, DateTimeKind.Utc).AddTicks(4607), true, "LoginRoot1", "111111", null, new Guid("20ce71dc-f322-4d31-a525-d567ec0e2c9a") });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("9cab7bf8-5c11-4815-8acc-9f1dbcca6981"), new DateTime(2019, 4, 12, 13, 27, 18, 619, DateTimeKind.Utc).AddTicks(8460), true, "LoginRoot2", "222222", null, new Guid("f8319492-31b4-4b44-a750-667838a114ee") });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "IsMainAccount", "MultiAccountName", "RefLink", "SponsorId", "UserAccountDataId" },
                values: new object[] { new Guid("5e02e395-4869-4d6a-83f5-55aa8650e372"), new DateTime(2019, 4, 12, 13, 27, 18, 619, DateTimeKind.Utc).AddTicks(8484), true, "LoginRoot3", "333333", null, new Guid("0ea1c2ff-b944-4aa8-98ec-a967cf7d7a65") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("0f7b51a5-dd67-4ac3-9409-6d2a558e750d"), new DateTime(2019, 4, 12, 13, 27, 18, 620, DateTimeKind.Utc).AddTicks(5333), 0, 1, 0, null, 14, new Guid("07d685e1-bed9-4598-8952-5a111f6fa15d") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("1aba1a60-41d0-4812-b9f2-51faa2ec60d8"), new DateTime(2019, 4, 12, 13, 27, 18, 620, DateTimeKind.Utc).AddTicks(6228), 1, 2, 0, new Guid("0f7b51a5-dd67-4ac3-9409-6d2a558e750d"), 7, new Guid("9cab7bf8-5c11-4815-8acc-9f1dbcca6981") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "CreatedAt", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("f1e4ba54-5d4d-47fa-8481-e790db06bf58"), new DateTime(2019, 4, 12, 13, 27, 18, 620, DateTimeKind.Utc).AddTicks(6251), 1, 8, 0, new Guid("0f7b51a5-dd67-4ac3-9409-6d2a558e750d"), 13, new Guid("5e02e395-4869-4d6a-83f5-55aa8650e372") });

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
                name: "UserMultiAccounts");

            migrationBuilder.DropTable(
                name: "UserAccountsData");
        }
    }
}
