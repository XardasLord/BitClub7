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
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
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
                    IsMembershipFeePaid = table.Column<bool>(nullable: false)
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
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    UserAccountDataId = table.Column<Guid>(nullable: false),
                    UserMultiAccountInvitingId = table.Column<Guid>(nullable: true),
                    MultiAccountName = table.Column<string>(nullable: true),
                    RefLink = table.Column<string>(nullable: true),
                    IsMainAccount = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMultiAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMultiAccounts_UserAccountsData_UserAccountDataId",
                        column: x => x.UserAccountDataId,
                        principalTable: "UserAccountsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMultiAccounts_UserMultiAccounts_UserMultiAccountInvitingId",
                        column: x => x.UserMultiAccountInvitingId,
                        principalTable: "UserMultiAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatrixPositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    UserMultiAccountId = table.Column<Guid>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    MatrixLevel = table.Column<int>(nullable: false),
                    Left = table.Column<int>(nullable: false),
                    Right = table.Column<int>(nullable: false),
                    DepthLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatrixPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatrixPositions_MatrixPositions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "MatrixPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatrixPositions_UserMultiAccounts_UserMultiAccountId",
                        column: x => x.UserMultiAccountId,
                        principalTable: "UserMultiAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("b40cb1fd-abe9-4cde-9fd9-e0a63f54f817"), "BtcWalletAddressRoot1", "CityRoot1", "CountryRoot1", "EmailRoot1", "FirstNameRoot1", "", true, "LastNameRoot1", "LoginRoot1", "Root", "", "StreetRoot1", "ZipCodeRoot1" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("6d338237-3001-45b4-bed9-4f45356cb238"), "BtcWalletAddressRoot2", "CityRoot2", "CountryRoot2", "EmailRoot2", "FirstNameRoot2", "", true, "LastNameRoot2", "LoginRoot2", "Root", "", "StreetRoot2", "ZipCodeRoot2" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("ee67e7e5-5169-4051-a0e5-577d3d1f1dcf"), "BtcWalletAddressRoot3", "CityRoot3", "CountryRoot3", "EmailRoot3", "FirstNameRoot3", "", true, "LastNameRoot3", "LoginRoot3", "Root", "", "StreetRoot3", "ZipCodeRoot3" });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "IsMainAccount", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("a36071ce-0849-42bd-933b-14fb6cc2175a"), true, "LoginRoot1", "111111", new Guid("b40cb1fd-abe9-4cde-9fd9-e0a63f54f817"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "IsMainAccount", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("6a2df1e2-3553-4f81-b73d-5ac257b00dbf"), true, "LoginRoot2", "222222", new Guid("6d338237-3001-45b4-bed9-4f45356cb238"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "IsMainAccount", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("bda0b804-4d3b-41eb-aed6-cce41ce53945"), true, "LoginRoot3", "333333", new Guid("ee67e7e5-5169-4051-a0e5-577d3d1f1dcf"), null });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("f02338ac-0bd9-48ce-af26-70229e5ebb94"), 0, 1, 0, null, 14, new Guid("a36071ce-0849-42bd-933b-14fb6cc2175a") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("1af2f050-ae6e-4749-9419-d20b61c8f794"), 1, 2, 0, new Guid("f02338ac-0bd9-48ce-af26-70229e5ebb94"), 7, new Guid("6a2df1e2-3553-4f81-b73d-5ac257b00dbf") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("d445b122-047b-444c-9238-c61fb2c28dda"), 1, 8, 0, new Guid("f02338ac-0bd9-48ce-af26-70229e5ebb94"), 13, new Guid("bda0b804-4d3b-41eb-aed6-cce41ce53945") });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("181a0761-ed8f-4c3b-993e-f69e6390cfbf"), 2, 3, 0, new Guid("1af2f050-ae6e-4749-9419-d20b61c8f794"), 4, null });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("bf75e510-5f20-43ed-b693-e5d9c5fefcbb"), 2, 5, 0, new Guid("1af2f050-ae6e-4749-9419-d20b61c8f794"), 6, null });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("975da247-9300-4afe-a70b-fdf0d342d295"), 2, 9, 0, new Guid("d445b122-047b-444c-9238-c61fb2c28dda"), 10, null });

            migrationBuilder.InsertData(
                table: "MatrixPositions",
                columns: new[] { "Id", "DepthLevel", "Left", "MatrixLevel", "ParentId", "Right", "UserMultiAccountId" },
                values: new object[] { new Guid("1c7f1963-4cf6-4430-897b-dd79521e6755"), 2, 11, 0, new Guid("d445b122-047b-444c-9238-c61fb2c28dda"), 12, null });

            migrationBuilder.CreateIndex(
                name: "IX_MatrixPositions_ParentId",
                table: "MatrixPositions",
                column: "ParentId");

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
                name: "IX_UserMultiAccounts_UserAccountDataId",
                table: "UserMultiAccounts",
                column: "UserAccountDataId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMultiAccounts_UserMultiAccountInvitingId",
                table: "UserMultiAccounts",
                column: "UserMultiAccountInvitingId");
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
