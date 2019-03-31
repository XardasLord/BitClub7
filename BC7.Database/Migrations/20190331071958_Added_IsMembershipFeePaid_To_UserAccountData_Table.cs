using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BC7.Database.Migrations
{
    public partial class Added_IsMembershipFeePaid_To_UserAccountData_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("12b6c47e-225c-4ee4-8a60-b7889901d2ff"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("716ea8fc-2ed6-4fbc-b522-2f09ea5f12fd"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("93f4f408-eca7-4916-8d8b-5507aede7463"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("48e60f3d-0c37-419d-94fe-420ee9800821"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("8c9ac8d5-3727-4611-a136-5afb687de595"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("edeb093c-2cf6-40aa-832c-7b5a707825f4"));

            migrationBuilder.AddColumn<bool>(
                name: "IsMembershipFeePaid",
                table: "UserAccountsData",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "IsMembershipFeePaid",
                table: "UserAccountsData");

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("8c9ac8d5-3727-4611-a136-5afb687de595"), "BtcWalletAddressRoot1", "CityRoot1", "CountryRoot1", "EmailRoot1", "FirstNameRoot1", "", "LastNameRoot1", "LoginRoot1", "Root", "", "StreetRoot1", "ZipCodeRoot1" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("48e60f3d-0c37-419d-94fe-420ee9800821"), "BtcWalletAddressRoot2", "CityRoot2", "CountryRoot2", "EmailRoot2", "FirstNameRoot2", "", "LastNameRoot2", "LoginRoot2", "Root", "", "StreetRoot2", "ZipCodeRoot2" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "Email", "FirstName", "Hash", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("edeb093c-2cf6-40aa-832c-7b5a707825f4"), "BtcWalletAddressRoot3", "CityRoot3", "CountryRoot3", "EmailRoot3", "FirstNameRoot3", "", "LastNameRoot3", "LoginRoot3", "Root", "", "StreetRoot3", "ZipCodeRoot3" });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("716ea8fc-2ed6-4fbc-b522-2f09ea5f12fd"), "LoginRoot1", "111111", new Guid("8c9ac8d5-3727-4611-a136-5afb687de595"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("93f4f408-eca7-4916-8d8b-5507aede7463"), "LoginRoot2", "222222", new Guid("48e60f3d-0c37-419d-94fe-420ee9800821"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("12b6c47e-225c-4ee4-8a60-b7889901d2ff"), "LoginRoot3", "333333", new Guid("edeb093c-2cf6-40aa-832c-7b5a707825f4"), null });
        }
    }
}
