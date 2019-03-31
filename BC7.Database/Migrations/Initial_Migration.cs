using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BC7.Database.Migrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccountsData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
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
                    Role = table.Column<string>(nullable: false)
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
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UserAccountDataId = table.Column<Guid>(nullable: false),
                    UserMultiAccountInvitingId = table.Column<Guid>(nullable: true),
                    AccountNumber = table.Column<int>(nullable: false),
                    RefLink = table.Column<string>(nullable: true)
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
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UserMultiAccountId = table.Column<Guid>(nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

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
