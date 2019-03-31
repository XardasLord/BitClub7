using Microsoft.EntityFrameworkCore.Migrations;

namespace BC7.Database.Migrations
{
    public partial class Renamed_Column_To_MultiAccountName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "UserMultiAccounts");

            migrationBuilder.AddColumn<string>(
                name: "MultiAccountName",
                table: "UserMultiAccounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MultiAccountName",
                table: "UserMultiAccounts");

            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                table: "UserMultiAccounts",
                nullable: false,
                defaultValue: 0);
        }
    }
}
