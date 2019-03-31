using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BC7.Database.Migrations
{
    public partial class Added_Default_Value_For_CreatedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("183bbc62-a871-488f-883d-970c09fb46ab"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("18567dcb-042a-4e7a-b34b-539c12b04aa3"));

            migrationBuilder.DeleteData(
                table: "UserMultiAccounts",
                keyColumn: "Id",
                keyValue: new Guid("1dd37daf-cb83-44a4-8c2c-2dd85d519b77"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("2637699b-902d-4c13-a32b-0eb85ecdd25b"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("b8f28295-923c-4616-aa46-81393ad7bdd2"));

            migrationBuilder.DeleteData(
                table: "UserAccountsData",
                keyColumn: "Id",
                keyValue: new Guid("e71c10a5-bcb4-4ca8-9ab1-e60467baf44f"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserMultiAccounts",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserAccountsData",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MatrixPositions",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime));

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserMultiAccounts",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserAccountsData",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "MatrixPositions",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "CreatedAt", "Email", "FirstName", "Hash", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("b8f28295-923c-4616-aa46-81393ad7bdd2"), "BtcWalletAddressRoot1", "CityRoot1", "CountryRoot1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EmailRoot1", "FirstNameRoot1", "", "LastNameRoot1", "LoginRoot1", "Root", "", "StreetRoot1", "ZipCodeRoot1" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "CreatedAt", "Email", "FirstName", "Hash", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("e71c10a5-bcb4-4ca8-9ab1-e60467baf44f"), "BtcWalletAddressRoot2", "CityRoot2", "CountryRoot2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EmailRoot2", "FirstNameRoot2", "", "LastNameRoot2", "LoginRoot2", "Root", "", "StreetRoot2", "ZipCodeRoot2" });

            migrationBuilder.InsertData(
                table: "UserAccountsData",
                columns: new[] { "Id", "BtcWalletAddress", "City", "Country", "CreatedAt", "Email", "FirstName", "Hash", "LastName", "Login", "Role", "Salt", "Street", "ZipCode" },
                values: new object[] { new Guid("2637699b-902d-4c13-a32b-0eb85ecdd25b"), "BtcWalletAddressRoot3", "CityRoot3", "CountryRoot3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EmailRoot3", "FirstNameRoot3", "", "LastNameRoot3", "LoginRoot3", "Root", "", "StreetRoot3", "ZipCodeRoot3" });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("18567dcb-042a-4e7a-b34b-539c12b04aa3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LoginRoot1", "111111", new Guid("b8f28295-923c-4616-aa46-81393ad7bdd2"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("183bbc62-a871-488f-883d-970c09fb46ab"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LoginRoot2", "222222", new Guid("e71c10a5-bcb4-4ca8-9ab1-e60467baf44f"), null });

            migrationBuilder.InsertData(
                table: "UserMultiAccounts",
                columns: new[] { "Id", "CreatedAt", "MultiAccountName", "RefLink", "UserAccountDataId", "UserMultiAccountInvitingId" },
                values: new object[] { new Guid("1dd37daf-cb83-44a4-8c2c-2dd85d519b77"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LoginRoot3", "333333", new Guid("2637699b-902d-4c13-a32b-0eb85ecdd25b"), null });
        }
    }
}
