using Microsoft.EntityFrameworkCore.Migrations;

namespace BC7.Database.Migrations
{
    public partial class AddedInitiativeDescriptionColumnToUserAccountData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InitiativeDescription",
                table: "UserAccountsData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitiativeDescription",
                table: "UserAccountsData");
        }
    }
}
