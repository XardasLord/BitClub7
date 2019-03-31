using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BC7.Database.Migrations
{
    public partial class Added_IsMainAccount_To_UserMultiAccounts_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("0af28994-1464-45c2-9353-377c278f0b49"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("22238166-a8d8-4ad2-93f8-37079e7607d1"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("66e981e5-1c6d-4078-aa96-89e4cb4359cb"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("a4000228-f30b-4214-aaef-680321a37737"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("aba8457d-38b2-4992-b3dc-e55fbbbd3cf9"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("bd40573d-a820-473d-b823-0facf4200f8c"));

            migrationBuilder.AddColumn<bool>(
                name: "IsMainAccount",
                table: "UserMultiAccounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("92454a8f-0cbd-4e14-8f27-e39d878a7239"), "BtcWalletAddressRoot1", "CityRoot1", "CountryRoot1", "EmailRoot1", "FirstNameRoot1", "", true, "LastNameRoot1", "LoginRoot1", "Root", "", "StreetRoot1", "ZipCodeRoot1" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("d9c7d725-596d-45ea-87c8-1598898094e3"), "BtcWalletAddressRoot2", "CityRoot2", "CountryRoot2", "EmailRoot2", "FirstNameRoot2", "", true, "LastNameRoot2", "LoginRoot2", "Root", "", "StreetRoot2", "ZipCodeRoot2" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("4af9960c-eb99-4cf6-9f51-d3da6faebca2"), "BtcWalletAddressRoot3", "CityRoot3", "CountryRoot3", "EmailRoot3", "FirstNameRoot3", "", true, "LastNameRoot3", "LoginRoot3", "Root", "", "StreetRoot3", "ZipCodeRoot3" });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "IsMainAccount", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("b5743183-0295-4297-b076-639b7b6e3ba0"), true, "LoginRoot1", "111111", new Guid("92454a8f-0cbd-4e14-8f27-e39d878a7239"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "IsMainAccount", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("0f48dbb7-4eda-4947-92e0-1764a932077a"), true, "LoginRoot2", "222222", new Guid("d9c7d725-596d-45ea-87c8-1598898094e3"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "IsMainAccount", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("9e94082c-343d-45c3-a854-34abe72769f8"), true, "LoginRoot3", "333333", new Guid("4af9960c-eb99-4cf6-9f51-d3da6faebca2"), null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("0f48dbb7-4eda-4947-92e0-1764a932077a"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("9e94082c-343d-45c3-a854-34abe72769f8"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("b5743183-0295-4297-b076-639b7b6e3ba0"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("4af9960c-eb99-4cf6-9f51-d3da6faebca2"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("92454a8f-0cbd-4e14-8f27-e39d878a7239"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("d9c7d725-596d-45ea-87c8-1598898094e3"));

            migrationBuilder.DropColumn(
                name: "IsMainAccount",
                table: "UserMultiAccounts");

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("bd40573d-a820-473d-b823-0facf4200f8c"), "BtcWalletAddressRoot1", "CityRoot1", "CountryRoot1", "EmailRoot1", "FirstNameRoot1", "", true, "LastNameRoot1", "LoginRoot1", "Root", "", "StreetRoot1", "ZipCodeRoot1" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("aba8457d-38b2-4992-b3dc-e55fbbbd3cf9"), "BtcWalletAddressRoot2", "CityRoot2", "CountryRoot2", "EmailRoot2", "FirstNameRoot2", "", true, "LastNameRoot2", "LoginRoot2", "Root", "", "StreetRoot2", "ZipCodeRoot2" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "IsMembershipFeePaid", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("a4000228-f30b-4214-aaef-680321a37737"), "BtcWalletAddressRoot3", "CityRoot3", "CountryRoot3", "EmailRoot3", "FirstNameRoot3", "", true, "LastNameRoot3", "LoginRoot3", "Root", "", "StreetRoot3", "ZipCodeRoot3" });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("66e981e5-1c6d-4078-aa96-89e4cb4359cb"), "LoginRoot1", "111111", new Guid("bd40573d-a820-473d-b823-0facf4200f8c"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("22238166-a8d8-4ad2-93f8-37079e7607d1"), "LoginRoot2", "222222", new Guid("aba8457d-38b2-4992-b3dc-e55fbbbd3cf9"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("0af28994-1464-45c2-9353-377c278f0b49"), "LoginRoot3", "333333", new Guid("a4000228-f30b-4214-aaef-680321a37737"), null });
        }
    }
}
